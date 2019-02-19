# 2D Tile water flow - Unity

A simple unity project with a 2D tilemap water flow algorithm implementation. A build of the projekt is available in the build folder.

**Build Demo**

In the demo, you choose a tile where the water will originate from. The height of that tile will be the max height that the water can fill up to. The algorithm will mimic normal water and will flow to the lowest point first. If the water fills up to a ridge and overflows, it will fill that new area until it reaches the same level as the rest of the water.

The demo has three heightmaps to test on and the heightmaps can be inverted. You can also draw your own heightmap inside the demo. Choose custom heightmap in the dropdown to get to the editor.