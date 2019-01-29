public class UnReach : BaseTile {
    public UnReach(ImageDirction dirction = ImageDirction.UP) {
        image = "Unreach";
        liitleMapImg = "Unreach_littleMap";
        this.dirction = dirction;
        reach = false;
    }
}
