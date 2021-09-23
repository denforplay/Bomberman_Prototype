using DG.Tweening;
using Models;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class EnemyController : MonoBehaviour
    {
        private readonly int X = Animator.StringToHash("x");
        private readonly int Y = Animator.StringToHash("y");
        private readonly int IsWalking = Animator.StringToHash("IsWalking");
        [SerializeField] private Animator _farmerAnimator;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private Tilemap _gameTilemap;
        [SerializeField] private Enemy _enemy;
        [SerializeField] private Rigidbody2D _enemyRigidbody;
        private Vector2[] _directions;
        private Tween _movingTween;
        private Vector2 _previousDirection;
        private Vector3 _previousPosition;

        private void Awake()
        {
            _enemyRigidbody.freezeRotation = true;
            var cellSize = _gameTilemap.cellSize;
            _directions = new[]
            {
                new Vector2(0, cellSize.y),
                new Vector2(0, -cellSize.y),
                new Vector2(cellSize.x, 0),
                new Vector2(-cellSize.x, 0),
            };
            
            Move(false);
        }

        private void Move(bool isLastOutOfZone)
        {
            _previousPosition = _enemyRigidbody.transform.position;
            var direction = _directions[Random.Range(0, _directions.Length)];
            while (direction == _previousDirection && isLastOutOfZone)
            {
                direction = _directions[Random.Range(0, _directions.Length)];
            }

            var enemyPos = _enemyRigidbody.transform.position;
            var nextPosition = new Vector2(enemyPos.x + direction.x, enemyPos.y + direction.y);
            var nextCellPos = _gameTilemap.WorldToCell(nextPosition);
            if (_gameTilemap.GetColliderType(nextCellPos) == Tile.ColliderType.None)
            {
                _farmerAnimator.SetFloat(X, direction.x);
                _farmerAnimator.SetFloat(Y, direction.y);
                _farmerAnimator.SetBool(IsWalking,true);
                
                _previousDirection = direction;
                Tween tween = _enemyRigidbody.transform
                    .DOMove(nextPosition, 1f)
                    .SetEase(_curve)
                    .SetUpdate(UpdateType.Fixed)
                    .OnComplete(() => Move(false));
            }
            else
            {
                Move(false);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out GameBorders gameBorders) || other.gameObject.TryGetComponent(out Bomb bomb))
            {
                DOTween.Kill(_enemyRigidbody.transform);
                _enemyRigidbody.transform.position = _previousPosition;
                Move(true);
            }
        }

        private void OnDestroy()
        {
            DOTween.Kill(transform);
        }
    }
}