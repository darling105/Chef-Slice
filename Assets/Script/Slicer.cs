using BzKovSoft.ObjectSlicerSamples;

using DG.Tweening;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    [SerializeField] private GameObject _knife;
    private float _duration = 0.03f;
    [SerializeField] private float _offsetY;

    private BzKnife _razor;
    private Coroutine _cutting;
    private bool _isWorking = false;
    private float _stunDuration;
    private Tween _currentTween; // Lưu tham chiếu đến tween hiện tại

    private void Awake()
    {
        _razor = _knife.GetComponentInChildren<BzKnife>();
    }

    private void Update()
    {
        if (_stunDuration > 0)
            _stunDuration -= Time.deltaTime;

        if (Input.GetMouseButton(0) && !_isWorking && _stunDuration <= 0)
        {
            _razor.BeginNewSlice();

            if (_cutting != null)
                StopCoroutine(_cutting);

            _cutting = StartCoroutine(Cut());
        }
    }

    public void Stun(float time)
    {
        _stunDuration = time;
    }

    private IEnumerator Cut()
    {
        _isWorking = true;

        // Kiểm tra knife vẫn tồn tại trước khi tạo tween
        if (_knife != null && _knife.transform != null)
        {
            // Hủy tween hiện tại trước khi tạo tween mới
            if (_currentTween != null && _currentTween.IsActive())
            {
                _currentTween.Kill();
            }

            SoundManager.Instance.PlayKnifeChopSFX();

            _currentTween = _knife.transform
                    .DOMoveY(_knife.transform.position.y - _offsetY, _duration)
                    .SetLoops(2, LoopType.Yoyo);

            yield return _currentTween.WaitForCompletion();
        }

        _isWorking = false;
    }

    private void OnDestroy()
    {
        // Hủy tween khi object này bị phá hủy
        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill();
        }
        
        // Dừng tất cả coroutines đang chạy
        if (_cutting != null)
        {
            StopCoroutine(_cutting);
        }
    }

    // Phương thức thay thế nếu cần dừng animation cutting từ bên ngoài
    public void StopCutting()
    {
        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill();
        }
        
        if (_cutting != null)
        {
            StopCoroutine(_cutting);
            _cutting = null;
        }
        
        _isWorking = false;
    }


}
