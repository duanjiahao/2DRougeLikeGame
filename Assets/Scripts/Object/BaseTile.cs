﻿public enum ImageDirction {
    UP = 1,
    DOWN = 2,
    LEFT = 3,
    RIGHT = 4,
}

public abstract class BaseTile {

    public string liitleMapImg;

    public string image;

    public ImageDirction dirction;

    public bool reach;
}
