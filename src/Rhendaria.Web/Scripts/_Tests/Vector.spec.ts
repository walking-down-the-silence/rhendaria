import "jasmine";
import { Vector2D as Vector } from "../Geometry/Vector2D";

describe("Vector2D", () => {
    it("can be created with 0 0",
        () => {
            // Arrange.
            const x = 0, y = 0;

            // Act.
            const target = Vector.create(x, y);

            // Assert.
            expect(target.x).toBe(x);
            expect(target.y).toBe(y);
        });

    it("add (0;0),(2;2) result in (2;2)",
        () => {
            // Arrange.
            const target = Vector.create(0, 0);
            const secondVector = Vector.create(2, 2);
            const resultX = 2, resultY = 2;

            // Act.
            const result = target.add(secondVector);

            // Assert.
            expect(result.x).toBe(resultX);
            expect(result.y).toBe(resultY);
        });

    it("shrink (2;2) by 2 result in (1;1)",
        () => {
            // Arrange.
            const target = Vector.create(2, 2);

            // Act.
            const result = target.shrink(2);

            // Assert.
            expect(result.x).toBe(1);
            expect(result.y).toBe(1);
        });
});