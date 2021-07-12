import React, { useEffect, useState } from "react";
import Video from './Video';
import { getVideosWithComments, searchVideos } from "../modules/videoManager";

const VideoList = () => {
    const [videos, setVideos] = useState([]);
    const [search, setSearch] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    const getVideos = () => {
        getVideosWithComments().then(videos => setVideos(videos));
    };

    const getFilteredVideos = () => {
        searchVideos(search, true).then(videos => setVideos(videos));
    }

    const handleControlledInputChange = (event) => {
        setIsLoading(false)
        let selectedVal = event.target.value
        if (event.target.id.includes("Id")) {
            selectedVal = parseInt(selectedVal)
        }
        setSearch(selectedVal)
    };

    const handleClickSearch = (event) => {
        event.preventDefault()
        getFilteredVideos(search)
    }

    useEffect(() => {
        getVideos();
    }, []);

    useEffect(() => {
        getFilteredVideos();
    }, []);


    if (videos.length === 0) {
        return (
            <div className="container">
                <div className="row justify-content-center">
                    <input type="text" placeholder="Search By Title" onChange={handleControlledInputChange} />
                    <button type="submit" className="btn btn-dark" onClick={handleClickSearch}>Search</button>
                </div>
                <div className="row justify-content-center">
                    <p>Sorry, no results found.</p>
                </div>
            </div>
        )
    }
    else {

        return (
            <div className="container">
                <div className="row justify-content-center">
                    <input type="text" placeholder="Search By Title" onChange={handleControlledInputChange} />
                    <button type="submit" className="btn btn-dark" onClick={handleClickSearch}>Search</button>
                </div>
                <div className="row justify-content-center">
                    {videos.map((video) => (
                        <Video video={video} key={video.id} />
                    ))}
                </div>
            </div>
        );
    }
};

export default VideoList;
