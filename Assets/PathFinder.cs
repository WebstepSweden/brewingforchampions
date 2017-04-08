using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour {

    private Grid grid = new Grid();

    public class Grid
    {

        public float maxX = 4.5f;
        public float minX = -4.5f;

        public float maxZ = 9.5f;
        public float minZ = -9.5f;

        public float barrelXLength = 1f;
        public float barrelZLength = 1f;

        public Grid()
        {
            List<List<Tile>> tiles = new List<List<Tile>>();
            var zIndexMax = (maxZ - minZ) / barrelZLength;
            var xIndexMax = (maxX - minX) / barrelXLength;
            for (var xIndex = 0; xIndex < xIndexMax + 1; xIndex++)
            {
                var xPoint = minX + xIndex * barrelXLength;
                var currentList = new List<Tile>();
                tiles.Add(currentList);

                for (var zIndex = 0; zIndex < xIndexMax + 1; zIndex++)
                {
                    var zPoint = minZ + zIndex * barrelZLength;
                    var tile = new Tile()
                    {
                        midPoint = new Vector3(xPoint, 1, zPoint),
                        xIndex = xIndex,
                        zIndex = zIndex,
                        occupied = false
                    };
                    currentList.Add(tile);
                }
            }
        }
    }

    public class Tile
    {
        public Vector3 midPoint;
        public bool occupied;
        public int xIndex;
        public int zIndex;
    }

}
