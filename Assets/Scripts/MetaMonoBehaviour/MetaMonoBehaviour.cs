using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaMonoBehaviour : MonoBehaviour
{
	private Animator _animator;			 			/// <summary>Animator Component.</summary>
	private AudioSource _audioSource;	 			/// <summary>AudioSource Component.</summary>
	private Avatar _avatar;				 			/// <summary>Avatar Component.</summary>
	private BoxCollider _boxCollider;	 			/// <summary>BoxCollider Component.</summary>
	private BoxCollider2D _boxCollider2D; 			/// <summary>BoxCollider2D Component.</summary>
	private CapsuleCollider _capsuleCollider; 		/// <summary>CapsuleCollider Component.</summary>
	private CapsuleCollider2D _capsuleCollider2D; 	/// <summary>CapsuleCollider2D Component.</summary>
	private CircleCollider2D _circleCollider2D; 	/// <summary>CircleCollider2D Component.</summary>
	private Cloth _cloth; 							/// <summary>Cloth Component.</summary>
	private Collider _collider; 					/// <summary>Collider Component.</summary>
	private EdgeCollider2D _edgeCollider2D; 		/// <summary>EdgeCollider2D Component.</summary>
	private RectTransform _rectTransform; 			/// <summary>RectTransform Component.</summary>
	private Renderer _renderer; 					/// <summary>Renderer Component.</summary>
	private Rigidbody _rigidbody; 		 			/// <summary>Rigidbody Component.</summary>
	private Rigidbody2D _rigidbody2D; 				/// <summary>Rigidbody2D Component.</summary>
	private MeshCollider _meshCollider; 			/// <summary>MeshCollider Component.</summary>
	private MeshRenderer _meshRenderer; 			/// <summary>MeshRenderer Component.</summary>
	private SphereCollider _sphereCollider; 		/// <summary>SphereCollider Component.</summary>

#region Getters/Setters:
	/// <summary>Gets animator Component.</summary>
	public Animator animator 
	{
		get
		{
			if(gameObject.Has<Animator>() && _animator == null)
			{
				_animator = GetComponent<Animator>();
			}

			return _animator;
		}
	}

	/// <summary>Gets audioSource Component.</summary>
	public AudioSource audioSource 
	{
		get
		{
			if(gameObject.Has<AudioSource>() && _audioSource == null)
			{
				_audioSource = GetComponent<AudioSource>();
			}

			return _audioSource;
		}
	}

	/// <summary>Gets avatar Component.</summary>
	public Avatar avatar 
	{
		get
		{
			if(gameObject.Has<Avatar>() && _avatar == null)
			{
				_avatar = GetComponent<Avatar>();
			}

			return _avatar;
		}
	}

	/// <summary>Gets boxCollider Component.</summary>
	public BoxCollider boxCollider 
	{
		get
		{
			if(gameObject.Has<BoxCollider>() && _boxCollider == null)
			{
				_boxCollider = GetComponent<BoxCollider>();
			}

			return _boxCollider;
		}
	}

	/// <summary>Gets boxCollider2D Component.</summary>
	public BoxCollider2D boxCollider2D 
	{
		get
		{
			if(gameObject.Has<BoxCollider2D>() && _boxCollider2D == null)
			{
				_boxCollider2D = GetComponent<BoxCollider2D>();
			}

			return _boxCollider2D;
		}
	}

	/// <summary>Gets capsuleCollider Component.</summary>
	public CapsuleCollider capsuleCollider 
	{
		get
		{
			if(gameObject.Has<CapsuleCollider>() && _capsuleCollider == null)
			{
				_capsuleCollider = GetComponent<CapsuleCollider>();
			}

			return _capsuleCollider;
		}
	}

	/// <summary>Gets capsuleCollider2D Component.</summary>
	public CapsuleCollider2D capsuleCollider2D 
	{
		get
		{
			if(gameObject.Has<CapsuleCollider2D>() && _capsuleCollider2D == null)
			{
				_capsuleCollider2D = GetComponent<CapsuleCollider2D>();
			}

			return _capsuleCollider2D;
		}
	}

	/// <summary>Gets circleCollider2D Component.</summary>
	public CircleCollider2D circleCollider2D 
	{
		get
		{
			if(gameObject.Has<CircleCollider2D>() && _circleCollider2D == null)
			{
				_circleCollider2D = GetComponent<CircleCollider2D>();
			}

			return _circleCollider2D;
		}
	}

	/// <summary>Gets cloth Component.</summary>
	public Cloth cloth 
	{
		get
		{
			if(gameObject.Has<Cloth>() && _cloth == null)
			{
				_cloth = GetComponent<Cloth>();
			}

			return _cloth;
		}
	}

	/// <summary>Gets collider Component.</summary>
	public Collider collider 
	{
		get
		{
			if(gameObject.Has<Collider>() && _collider == null)
			{
				_collider = GetComponent<Collider>();
			}

			return _collider;
		}
	}

	/// <summary>Gets edgeCollider2D Component.</summary>
	public EdgeCollider2D edgeCollider2D 
	{
		get
		{
			if(gameObject.Has<EdgeCollider2D>() && _edgeCollider2D == null)
			{
				_edgeCollider2D = GetComponent<EdgeCollider2D>();
			}

			return _edgeCollider2D;
		}
	}

	/// <summary>Gets rectTransform Component.</summary>
	public RectTransform rectTransform 
	{
		get
		{
			if(gameObject.Has<RectTransform>() && _rectTransform == null)
			{
				_rectTransform = GetComponent<RectTransform>();
			}

			return _rectTransform;
		}
	}

	/// <summary>Gets renderer Component.</summary>
	public Renderer renderer 
	{
		get
		{
			if(gameObject.Has<Renderer>() && _renderer == null)
			{
				_renderer = GetComponent<Renderer>();
			}

			return _renderer;
		}
	}

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody rigidbody 
	{
		get
		{
			if(gameObject.Has<Rigidbody>() && _rigidbody == null)
			{
				_rigidbody = GetComponent<Rigidbody>();
			}

			return _rigidbody;
		}
	}

	/// <summary>Gets rigidbody2D Component.</summary>
	public Rigidbody2D rigidbody2D 
	{
		get
		{
			if(gameObject.Has<Rigidbody2D>() && _rigidbody2D == null)
			{
				_rigidbody2D = GetComponent<Rigidbody2D>();
			}

			return _rigidbody2D;
		}
	}

	/// <summary>Gets meshCollider Component.</summary>
	public MeshCollider meshCollider 
	{
		get
		{
			if(gameObject.Has<MeshCollider>() && _meshCollider == null)
			{
				_meshCollider = GetComponent<MeshCollider>();
			}

			return _meshCollider;
		}
	}

	/// <summary>Gets meshRenderer Component.</summary>
	public MeshRenderer meshRenderer 
	{
		get
		{
			if(gameObject.Has<MeshRenderer>() && _meshRenderer == null)
			{
				_meshRenderer = GetComponent<MeshRenderer>();
			}

			return _meshRenderer;
		}
	}

	/// <summary>Gets sphereCollider Component.</summary>
	public SphereCollider sphereCollider 
	{
		get
		{
			if(gameObject.Has<SphereCollider>() && _sphereCollider == null)
			{
				_sphereCollider = GetComponent<SphereCollider>();
			}

			return _sphereCollider;
		}
	}
#endregion
}
