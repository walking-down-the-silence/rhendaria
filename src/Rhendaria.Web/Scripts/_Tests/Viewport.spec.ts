import { Viewport } from "../Viewport";
describe("Viewport",() => {
    it('get center',
        () => {
            // Arrange.
            const viewport = Viewport.create(100, 100);

            // Act.
            const center = viewport.getCenter();

            // Assert.
            expect(center.x).toBe(50);
            expect(center.y).toBe(50);
        });
});