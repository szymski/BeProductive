window.LoadAnimation = function(path, containerId) {
    const anim = bodymovin.loadAnimation({
        container: document.getElementById(containerId),
        path: path,
        renderer: 'svg',
        autoplay: false,
        loop: false,
    });
    anim.playShot = () => {
        anim.stop();
        anim.play();
    };
    return anim;
}