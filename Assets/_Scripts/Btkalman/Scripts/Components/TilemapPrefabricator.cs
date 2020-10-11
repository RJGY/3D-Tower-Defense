using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Btkalman.Components {
    [RequireComponent(typeof(Tilemap), typeof(TilemapRenderer))]
    public class TilemapPrefabricator : MonoBehaviour {
        public interface IArguments {
            void SetArgs(GameObject gameObjectArg, string stringArg);
        }

        [System.Serializable]
        public struct Entry {
            [Tooltip("None for the default entry, required for other entries")] public TileBase tile;
            [Tooltip("Required")] public GameObject prefab;
            [Tooltip("Optional")] public GameObject gameObjectArg;
            [Tooltip("Optional")] public string stringArg;
        }
        [SerializeField] private Entry defaultEntry = default;
        [SerializeField] private Entry[] entries = null;
        [SerializeField] private bool disableTilemapRenderer = false;

        private Grid grid;
        private Tilemap tilemap;
        private TilemapRenderer tilemapRenderer;
        private List<GameObject> instantiatedPrefabs;

        private void Awake() {
            grid = transform.parent.GetComponent<Grid>();
            if (!grid) {
                Debug.LogError("[TilemapPrefabricator]: must have a Grid parent");
            }
            tilemap = GetComponent<Tilemap>();
            tilemapRenderer = GetComponent<TilemapRenderer>();
            instantiatedPrefabs = new List<GameObject>();
        }

        private void Start() {
            if (!grid) {
                return;
            }

            var tilemapBounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(tilemapBounds);
            for (int x = 0; x < tilemapBounds.size.x; x++) {
                for (int y = 0; y < tilemapBounds.size.y; y++) {
                    var tileBase = allTiles[x + y * tilemapBounds.size.x];
                    if (tileBase != null) {
                        var tileBasePos = new Vector3Int(tilemapBounds.x + x, tilemapBounds.y + y, 0);
                        var sprite = tilemap.GetSprite(tileBasePos);
                        if (sprite != null) {
                            // CellToWorld gives the lower left corner (i.e. min)
                            // world position of the tile. Convert it to the center
                            // of the tile.
                            var pos = grid.CellToWorld(tileBasePos) + new Vector3(
                                grid.transform.localScale.x / 2,
                                grid.transform.localScale.y / 2,
                                0);
                            InstantiatePrefab(tileBase, sprite, pos);
                        } else {
                            Debug.LogErrorFormat(
                                "[TilemapPrefabricator]: no sprite for tile {0} at {1}",
                                tileBase.name, tileBasePos);
                        }
                    }
                }
            }

            if (disableTilemapRenderer) {
                tilemapRenderer.enabled = false;
            }
        }

        private void InstantiatePrefab(TileBase tileBase, Sprite sprite, Vector3 pos) {
            Entry entry = defaultEntry;

            foreach (var e in entries) {
                if (e.tile == tileBase) {
                    entry = e;
                    break;
                }
            }

            if (!entry.prefab) {
                Debug.LogWarningFormat("[TilemapPrefabricator]: no prefab for {0}", sprite.name);
                return;
            }

            var instance = GameObject.Instantiate(entry.prefab, transform);
            instance.name = sprite.name;
            instance.transform.position = pos;
            foreach (var tp in instance.GetComponents<IArguments>()) {
                tp.SetArgs(entry.gameObjectArg, entry.stringArg);
            }
            instantiatedPrefabs.Add(instance);
        }

        private void OnDestroy() {
            foreach (var instance in instantiatedPrefabs) {
                GameObject.Destroy(instance);
            }
            instantiatedPrefabs.Clear();
        }
    }
}