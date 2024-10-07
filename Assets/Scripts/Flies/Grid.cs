using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Grid {
	int[][][] grid;
	int[][] gridCounts;

	int width;
	int height;
	int cellSize;

	/** width and height are the dimensions of the grid in cells. */
	public Grid(int widthRadius, int heightRadius, int cellSize) {
		this.width = widthRadius * 2;
		this.height = heightRadius * 2;
		this.cellSize = cellSize;
		grid = new int[width][][];
		gridCounts = new int[width][];
		for (int i = 0; i < width; i++) {
			grid[i] = new int[height][];
			gridCounts[i] = new int[height];
			for (int j = 0; j < height; j++) {
				grid[i][j] = new int[1000];
				gridCounts[i][j] = 0;
			}
		}
	}

	/**
	 * Insert a fly's index into the grid.
	 */
	public void insert(float x, float y, int index) {
		int gridX = getGridX(x);
		int gridY = getGridY(y);
		// Bounds check
		if (gridX < 0 || gridX >= width || gridY < 0 || gridY >= height) {
			return;
		}
		grid[gridX][gridY][gridCounts[gridX][gridY]++] = index;
	}

	public void fill(Fly[] flies, int flyCount) {
		for (int i = 0; i < flyCount; i++) {
			insert(flies[i].x, flies[i].y, i);
		}
	}

	public void clear() {
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				gridCounts[i][j] = 0;
			}
		}
	}

	int getGridX(float x) {
		return Mathf.RoundToInt((x + width / 2) / cellSize);
	}

	int getGridY(float y) {
		return Mathf.RoundToInt((y + height / 2) / cellSize);
	}

	public void query(float x, float y, float radius, List<int> neighbors) {
		neighbors.Clear();
		int gridX = getGridX(x);
		int gridY = getGridY(y);
		int radiusCells = (int) (radius / cellSize) + 1;
		for (int i = -radiusCells; i <= radiusCells; i++) {
			for (int j = -radiusCells; j <= radiusCells; j++) {
				int gx = gridX + i;
				int gy = gridY + j;
				if (gx >= 0 && gx < width && gy >= 0 && gy < height) {
					for (int k = 0; k < gridCounts[gx][gy]; k++) {
						neighbors.Add(grid[gx][gy][k]);
					}
				}
			}
		}
	}
}