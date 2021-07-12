using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Streamish.Models;
using Streamish.Utils;

namespace Streamish.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT Id, [Name], Email, ImageUrl, DateCreated, isDeleted FROM UserProfile WHERE isDeleted=0 ";

                    var reader = cmd.ExecuteReader();

                    var profiles = new List<UserProfile>();
                    while (reader.Read())
                    {
                        profiles.Add(new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated")
                        });
                    }
                    reader.Close();
                    return profiles;
                }
            }
        }
        public List<UserProfile> GetVideosByUserProfileId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.id, up.Name, up.Email, up.DateCreated AS UserProfileDateCreated,
                               up.ImageUrl AS UserProfileImageUrl,
                               v.Id AS VideoId, v.Title, v.Description, v.Url, 
                               v.DateCreated AS VideoDateCreated, v.UserProfileId As VideoUserProfileId
                       FROM UserProfile up
                       JOIN Video v on up.id = v.UserProfileId
                       WHERE up.id = @id";
                    DbUtils.AddParameter(cmd, "@Id", id);
                    var reader = cmd.ExecuteReader();

                    var profiles = new List<UserProfile>();
                    while (reader.Read())
                    {
                        var profileId = DbUtils.GetInt(reader, "id");

                        var existingProfile = profiles.FirstOrDefault(p => p.Id == profileId);
                        if (existingProfile == null)
                        {
                            existingProfile = new UserProfile()
                            {
                                Id = profileId,
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                                Videos = new List<Video>()
                            };
                            profiles.Add(existingProfile);

                        }
                        if (DbUtils.IsNotDbNull(reader, "VideoId"))
                        {
                            existingProfile.Videos.Add(new Video()
                            {
                                Id = DbUtils.GetInt(reader, "VideoId"),
                                Title = DbUtils.GetString(reader, "Title"),
                                Description = DbUtils.GetString(reader, "Description"),
                                Url = DbUtils.GetString(reader, "Url"),
                                DateCreated = DbUtils.GetDateTime(reader, "VideoDateCreated")
                            });
                        }
                    }
                    reader.Close();

                    return profiles;
                }
            }
        }
        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], Email, ImageUrl, DateCreated FROM UserProfile WHERE Id = @id ";
                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    UserProfile profile = null;
                    if (reader.Read())
                    {
                        profile = new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "Id"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated")
                        };
                    }

                    reader.Close();

                    return profile;
                }
            }
        }

        public void Add(UserProfile profile)
        {

            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            INSERT INTO UserProfile ( [Name], Email, ImageUrl, DateCreated)
                            OUTPUT INSERTED.ID
                            VALUES(@Name, @Email, @ImageUrl, @DateCreated)";

                    DbUtils.AddParameter(cmd, "@Name", profile.Name);
                    DbUtils.AddParameter(cmd, "@Email", profile.Email);
                    DbUtils.AddParameter(cmd, "@ImageUrl", profile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@DateCreated", profile.DateCreated);

                    profile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile profile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE UserProfile
                            SET [Name]=@Name,
                                Email=@Email,
                                ImageUrl=@ImageUrl,
                                DateCreated=@DateCreated
                                WHERE Id =@id";
                    DbUtils.AddParameter(cmd, "@Name", profile.Name);
                    DbUtils.AddParameter(cmd, "@Email", profile.Email);
                    DbUtils.AddParameter(cmd, "@ImageUrl", profile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@DateCreated", profile.DateCreated);
                    DbUtils.AddParameter(cmd, "@id", profile.Id);
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE UserProfile
                                        SET isDeleted=1
                                        WHERE Id= @id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}