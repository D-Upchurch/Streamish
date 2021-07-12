import React, { useState, useEffect } from "react";
import { useHistory } from "react-router-dom";
import { addVideo, getVideosWithComments } from "../modules/videoManager";

const VideoForm = () => {
    const [video, setVideo] = useState({
        title: "",
        description: "",
        url: ""
    });
    const [isLoading, setIsLoading] = useState(false);
    const history = useHistory();

    const handleFieldChange = (event) => {
        const newVideo = { ...video }
        let selectedVal = event.target.value
        if (event.target.id.includes("Id")) {
            selectedVal = parseInt(selectedVal)
        }
        newVideo[event.target.id] = selectedVal
        setVideo(newVideo)
    };

    const handleClickSaveVideo = (event) => {
        event.preventDefault()
        addVideo(video)
            .then(() => setVideo({
                title: "",
                description: "",
                url: ""
            })).then(() => history.push("/videos"))
    };

    return (
        <form className="videoForm">
            <h2 className="videoForm__title">New Video</h2>
            <fieldset>
                <div>
                    <input
                        type="hidden"
                        id="dateCreated"
                        onChange={handleFieldChange}
                        required autoFocus
                        className="form-control"
                        value={video.dateCreated} />
                </div>
            </fieldset>
            <fieldset>
                <div>
                    <input
                        type="hidden"
                        id="userProfileId"
                        onChange={handleFieldChange}
                        required autoFocus
                        className="form-control"
                        value={video.userProfileId} />
                </div>
            </fieldset>
            <fieldset>
                <div className="form-group">
                    <label htmlFor="title">Title:</label>
                    <input
                        type="text"
                        id="title"
                        onChange={handleFieldChange}
                        required autoFocus
                        className="form-control"
                        placeholder="Video Title"
                        value={video.title} />
                </div>
            </fieldset>
            <fieldset>
                <div className="form-group">
                    <label htmlFor="description">Description:</label>
                    <input
                        type="text"
                        id="description"
                        onChange={handleFieldChange}
                        required autoFocus
                        className="form-control"
                        placeholder="Description"
                        value={video.description} />
                </div>
            </fieldset>
            <fieldset>
                <div className="form-group">
                    <label htmlFor="url">URL:</label>
                    <input
                        type="text"
                        id="url"
                        onChange={handleFieldChange}
                        required autoFocus
                        className="form-control"
                        placeholder="URL"
                        value={video.url} />
                </div>
            </fieldset>
            <button className="btn btn-primary"
                onClick={handleClickSaveVideo}>
                Save Video
            </button>
        </form>
    )
}

export default VideoForm;