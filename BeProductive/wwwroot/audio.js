window.PlayAudio = function (url) {
    const audio = new Audio(url);
    return audio.play();
};