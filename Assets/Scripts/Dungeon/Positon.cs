using System;

public struct Position : IEquatable<Position> {
    public int row;
    public int col;

    public Position(int row, int col) {
        this.row = row;
        this.col = col;
    }

    public static bool operator == (Position position1, Position position2) {
        return position1.col == position2.col && position1.row == position2.row;
    }

    public static bool operator != (Position position1, Position position2) {
        return position1.col != position2.col || position1.row != position2.row;
    }

    public static Position operator + (Position position1, Position position2) {
        return new Position(position1.row + position2.row, position1.col + position2.col);
    }

    public static Position operator - (Position position1, Position position2) {
        return new Position(position1.row - position2.row, position1.col - position2.col);
    }

    public static Position operator * (Position position1, int mul) {
        return new Position(position1.row * mul, position1.col * mul);
    }

    public static Position operator / (Position position1, int mul) {
        return new Position(position1.row / mul, position1.col / mul);
    }

    public static bool operator > (Position position1, Position position2) {
        return position1.row > position2.row && position1.col > position2.col;
    }

    public static bool operator >= (Position position1, Position position2) {
        return position1.row >= position2.row && position1.col >= position2.col;
    }

    public static bool operator < (Position position1, Position position2) {
        return position1.row < position2.row && position1.col < position2.col;
    }

    public static bool operator <= (Position position1, Position position2) {
        return position1.row <= position2.row && position1.col <= position2.col;
    }

    public bool Equals(Position other) {
        return this.row.Equals(other.row) && this.col.Equals(other.col);
    }

    public Position Left() {
        return new Position(this.row, this.col - 1);
    }

    public Position Right() {
        return new Position(this.row, this.col + 1);
    }

    public Position Bottom() {
        return new Position(this.row - 1, this.col);
    }

    public Position Top() {
        return new Position(this.row + 1, this.col);
    }

    public int totalCount() {
        return Math.Abs(this.row) + Math.Abs(this.col);
    }

    public readonly static Position One = new Position(1, 1);

    public readonly static Position Zero = new Position(0, 0);

    public readonly static Position P01 = new Position(0, 1);

    public readonly static Position P10 = new Position(1, 0);

    public override bool Equals(object obj) {
        return base.Equals(obj);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }
}
