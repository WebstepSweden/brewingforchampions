using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour {

    private Grid grid = new Grid();

    public Vector3 GetNextGrid(string barrelId, Vector3 position, Vector3 target)
    {
        //print("position: " + JsonUtility.ToJson(position));
        var currentTile = grid.getTile(position);
        print("Current: " + JsonUtility.ToJson(currentTile.midPoint));
        var targetTile = grid.getTile(target);
        print("Target: " + JsonUtility.ToJson(targetTile.midPoint));

        if (targetTile.xIndex == currentTile.xIndex)
        {
            bool zDirectionIsPositive = currentTile.zIndex < targetTile.zIndex;
            
            var nextZ = zDirectionIsPositive ? currentTile.zIndex + 1 : currentTile.zIndex - 1;

            var firstCandidate = grid.getTile(currentTile.xIndex, nextZ);
            return firstCandidate.midPoint;
        } else
        {
            bool xDirectionIsPositive = currentTile.xIndex < targetTile.xIndex;

            var nextX = xDirectionIsPositive ? currentTile.xIndex + 1 : currentTile.xIndex - 1;

            var firstCandidate = grid.getTile(nextX, currentTile.zIndex);
            return firstCandidate.midPoint;
        }


    }



    public class Grid
    {

        public float maxX = 4.5f;
        public float minX = -4.5f;

        public float maxZ = 9.5f;
        public float minZ = -9.5f;

        public float barrelXLength = 1f;
        public float barrelZLength = 1f;
        List<List<Tile>> tiles;

        public Grid()
        {
            tiles = new List<List<Tile>>();
            var zIndexMax = (maxZ - minZ) / barrelZLength;
            var xIndexMax = (maxX - minX) / barrelXLength;
            for (var xIndex = 0; xIndex < xIndexMax + 1; xIndex++)
            {
                var xPoint = minX + xIndex * barrelXLength;
                var currentList = new List<Tile>();
                tiles.Add(currentList);

                for (var zIndex = 0; zIndex < zIndexMax + 1; zIndex++)
                {
                    var zPoint = minZ + zIndex * barrelZLength;
                    var tile = new Tile()
                    {
                        midPoint = new Vector3(xPoint, 1, zPoint),
                        xIndex = xIndex,
                        zIndex = zIndex,
                        occupiedBy = null
                    };
                    currentList.Add(tile);
                }
            }
        }

        public Tile getTile(int x, int z)
        {
            print("TILE. X: " + x + " Z: " + z);
            return tiles[x][z];
        }

        public Tile getTile(Vector3 pos)
        {
            int xIndex = (int)System.Math.Round(pos.x - minX, 0);
            int zIndex = (int)System.Math.Round(pos.z - minZ, 0);

            return tiles[xIndex][zIndex];
        }
    }

    internal Vector3 AllocateEnd(string id, Vector3 target)
    {
        return grid.getTile(target).midPoint;
    }

    public class Tile
    {
        public Vector3 midPoint;
        public string occupiedBy;
        public int xIndex;
        public int zIndex;
        public string reservedAsTargetBy;
    }

}
