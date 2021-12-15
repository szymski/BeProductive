window.LoadAnimation = function (path, container) {
    const anim = bodymovin.loadAnimation({
        container: container,
        path: path,
        renderer: "svg",
        autoplay: false,
        loop: false,
        rendererSettings: {
            progressiveLoad: true,
            // hideOnTransparent: true,
        }
    });

    anim.addEventListener("complete", () => {
        anim.stop();
    });

    anim.playShot = () => {
        anim.stop();
        anim.play();
    };

    return anim;
}