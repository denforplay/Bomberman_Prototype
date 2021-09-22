using Managers;
using Models.Particles;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace Models
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private BombParticle _bombParticle;
        [SerializeField] private GameObject _particlesContainer;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private int _bombsDistance = 1;
        private ParticleManager _particleManager;
        private Tilemap _gameTilemap;
        private Vector2[] _directions;

        [Inject]
        public void Init(ParticleManager particleManager)
        {
            _particleManager = particleManager;
        }

        public void SetTileMap(Tilemap tilemap)
        {
            _gameTilemap = tilemap;
        }
        
        private void OnDestroy()
        {
            _particleManager.GetParticle(typeof(BombParticle)).transform.position = transform.position;
            var cellSize = _gameTilemap.cellSize;
            _directions = new[]
            {
                new Vector2(0, cellSize.y),
                new Vector2(0, -cellSize.y),
                new Vector2(cellSize.x, 0),
                new Vector2(-cellSize.x, 0),
            };
            
            foreach (var direction in _directions)
            {
                for (int i = 1; i <= _bombsDistance; i++)
                {
                    var nextPosition = new Vector2(transform.position.x + direction.x * i, transform.position.y + direction.y * i);
                    var nextCellPos = _gameTilemap.WorldToCell(nextPosition);
                    if (_gameTilemap.GetColliderType(nextCellPos) == Tile.ColliderType.None)
                    {
                        var _bombParticle = _particleManager.GetParticle(typeof(BombParticle));
                        _bombParticle.transform.position = nextPosition;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            _collider.isTrigger = false;
        }
        
        public class BombFactory : PlaceholderFactory<Bomb>
        {
        }
    }
}