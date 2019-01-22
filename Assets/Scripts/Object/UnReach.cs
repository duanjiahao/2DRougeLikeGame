public class UnReach : BaseTile {
    public UnReach(ImageDirction dirction = ImageDirction.UP) {
        image = "Unreach";
        this.dirction = dirction;
        reach = false;
    }
}
