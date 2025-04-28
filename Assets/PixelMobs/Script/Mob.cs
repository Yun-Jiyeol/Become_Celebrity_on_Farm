using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Mob : MonoBehaviour
{
    public string Skin = "A";

    Sprite[] _sprites;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    string _path;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        Load();
    }

    void Load()
    {
        if (_animator == null || _animator.runtimeAnimatorController == null)
            return;

        var path = "Mob/" + _animator.runtimeAnimatorController.name + Skin;

        if (!_path.Equals(path))
        {
            _path = path;
            _sprites = Resources.LoadAll<Sprite>(_path);

            // 만약 스프라이트를 못 찾으면 경고 출력
            if (_sprites == null || _sprites.Length == 0)
                Debug.LogWarning("No sprites found at path: " + path);
        }
    }

    void LateUpdate()
    {
        if (_spriteRenderer == null || _spriteRenderer.sprite == null)
            return;

        Load();
        var name = _spriteRenderer.sprite.name;
        var sprite = Array.Find(_sprites, item => item.name == name);
        if (sprite)
            _spriteRenderer.sprite = sprite;
    }
}

//using System;
//using UnityEngine;
//using UnityEngine.UI;
//[ExecuteInEditMode]
//[RequireComponent(typeof(Image))]
//[RequireComponent(typeof(Animator))]
//public class Mob : MonoBehaviour
//{
//	public string Skin = "A";
//	Sprite[] _sprites;
//	Image _image;
//	Animator _animator;
//	string _path;
//	void Awake()
//	{
//		_image = GetComponent<Image>();
//		_animator = GetComponent<Animator>();
//		Load();
//	}
//	void Load()
//	{
//		var path = "Mob/" + _animator.runtimeAnimatorController.name + Skin;
//		if (!path.Equals(_path))
//		{
//			_path = path;
//			_sprites = Resources.LoadAll<Sprite>(_path);
//		}
//	}
//	void LateUpdate()
//	{
//		if (_image == null || _image.sprite == null)
//			return;
//		Load();
//		var name = _image.sprite.name;
//		var sprite = Array.Find(_sprites, item => item.name == name);
//		if (sprite)
//			_image.sprite = sprite;
//	}
//}
