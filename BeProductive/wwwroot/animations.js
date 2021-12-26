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
    
    let removeOnFinish = false;

    anim.addEventListener("complete", () => {
        if(removeOnFinish) {
            anim.stop();
        }
    });

    anim.playShot = (remove) => {
        removeOnFinish = remove;
        anim.stop();
        anim.play();
    };

    return anim;
}