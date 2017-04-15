
/// Filter any shortcut that your project need. (SEE DOCS or below code)

#define CORE_FEATURES
#define EXTRA_FEATURES

#if CORE_FEATURES
#define EX_CORE
#define EX_PHYSICS
#define EX_UI
#endif

#if EXTRA_FEATURES
#define EX_VISUAL
#define EX_AUDIOFILTERS
#define EX_COLLIDERS
#define EX_JOINTS
#define EX_COLLIDERS2D
#define EX_JOINTS2D
#define EX_UI_VISUAL
#define EX_UI_INTERACTION
#define EX_UI_LAYOUT
#endif

using UnityEngine;
using UnityEngine.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections;

public class MonoBehaviourEX : MonoBehaviour
{
    /// <summary>
    /// Determine if EX shortcuts is allowed to look in children. 
    /// NOTE: Always Set this before accessing any Shortcuts, either in Awake() or OnEnable() 
    /// </summary>
    [NonSerialized] 
    public bool EXLooksInChildren = false;
    
    private T _GOAC<T>() where T : UnityEngine.Component { return (EXLooksInChildren ? GetComponentInChildren<T>() : null) ?? gameObject.GetOrAddComponent<T>(); }

    public T GC<T>() where T : UnityEngine.Component { return (T)GetComponent(typeof(T)); }

    public T GAC<T>() where T : UnityEngine.Component { return gameObject.GetOrAddComponent<T>(); }

    // Make shortcut doesn't spam your debugger
    private const DebuggerBrowsableState _NEV = DebuggerBrowsableState.Never;
    private const EditorBrowsableState _ADV = EditorBrowsableState.Advanced;
#if EX_CORE

    [NonSerialized] private GameObject _go; /// <summary> Get cached GameObject </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public GameObject GO { get { return _go ?? (_go = gameObject);} }
    
    [NonSerialized] private MeshFilter _mf; /// <summary> Get a MeshFilter Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public MeshFilter MF { get { return _mf ?? (_mf = _GOAC<MeshFilter>());} }
    
    [NonSerialized] private MeshRenderer _mr; /// <summary> Get a MeshRenderer Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public MeshRenderer MR { get { return _mr ?? (_mr = _GOAC<MeshRenderer>());} }
        
    [NonSerialized] private AudioSource _as; /// <summary> Get a AudioSource Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public AudioSource AS { get { return _as ?? (_as = _GOAC<AudioSource>());} }
      
    [NonSerialized] private Light _l; /// <summary> Get a Light Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Light L { get { return _l ?? (_l = _GOAC<Light>());} }
    
    [NonSerialized] private Camera _cm; /// <summary> Get a Camera Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Camera CM { get { return _cm ?? (_cm = _GOAC<Camera>());} }

    [NonSerialized] private Transform _t; /// <summary> Get a Transform Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Transform T { get { return _t ?? (_t = _GOAC<Transform>());} }
 #endif

#if EX_VISUAL 
    [NonSerialized] private ParticleSystem _ps; /// <summary> Get a ParticleSystem Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public ParticleSystem PS { get { return _ps ?? (_ps = _GOAC<ParticleSystem>());} }
    
    [NonSerialized] private Animator _an; /// <summary> Get a Animator Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Animator AN { get { return _an ?? (_an = _GOAC<Animator>());} }
    
    [NonSerialized] private LineRenderer _lr; /// <summary> Get a LineRenderer Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public LineRenderer LR { get { return _lr ?? (_lr = _GOAC<LineRenderer>());} }

    [NonSerialized] private TrailRenderer _tr; /// <summary> Get a TrailRenderer Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public TrailRenderer TR { get { return _tr ?? (_tr = _GOAC<TrailRenderer>());} }

    [NonSerialized] private ReflectionProbe _rp; /// <summary> Get a ReflectionProbe Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public ReflectionProbe RP { get { return _rp ?? (_rp = _GOAC<ReflectionProbe>());} }

    [NonSerialized] private LODGroup _lg; /// <summary> Get a LODGroup Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public LODGroup LG { get { return _lg ?? (_lg = _GOAC<LODGroup>());} }
 #endif
#if EX_PHYSICS
    [NonSerialized] private Rigidbody _rb; /// <summary> Get a Rigidbody Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Rigidbody RB { get { return _rb ?? (_rb = _GOAC<Rigidbody>());} }
    
    [NonSerialized] private Collider _x; /// <summary> Get a Collider Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Collider X { get { return _x ?? (_x = GetComponent<Collider>());} }
    
    [NonSerialized] private Rigidbody2D _rb2d; /// <summary> Get a Rigidbody2D Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Rigidbody2D RB2D { get { return _rb2d ?? (_rb2d = _GOAC<Rigidbody2D>());} }
    
    [NonSerialized] private Collider2D _x2d; /// <summary> Get a Collider2D Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Collider2D X2D { get { return _x2d ?? (_x2d = GetComponent<Collider2D>());} }
 #endif
 
#if EX_COLLIDERS
    [NonSerialized] private BoxCollider _xb; /// <summary> Get a BoxCollider Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public BoxCollider XB { get { return _xb ?? (_xb = _GOAC<BoxCollider>());} }
    
    [NonSerialized] private SphereCollider _xs; /// <summary> Get a SphereCollider Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public SphereCollider XS { get { return _xs ?? (_xs = _GOAC<SphereCollider>());} }
    
    [NonSerialized] private CapsuleCollider _xc; /// <summary> Get a CapsuleCollider Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public CapsuleCollider XC { get { return _xc ?? (_xc = _GOAC<CapsuleCollider>());} }
    
    [NonSerialized] private MeshCollider _xm; /// <summary> Get a MeshCollider Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public MeshCollider XM { get { return _xm ?? (_xm = _GOAC<MeshCollider>());} }
    
    [NonSerialized] private TerrainCollider _xt; /// <summary> Get a TerrainCollider Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public TerrainCollider XT { get { return _xt ?? (_xt = _GOAC<TerrainCollider>());} }
 #endif
#if EX_JOINTS

    [NonSerialized] private CharacterJoint _jc; /// <summary> Get a CharacterJoint Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public CharacterJoint JC { get { return _jc ?? (_jc = _GOAC<CharacterJoint>());} }

    [NonSerialized] private HingeJoint _jh; /// <summary> Get a HingeJoint Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public HingeJoint JH { get { return _jh ?? (_jh = _GOAC<HingeJoint>());} }

    [NonSerialized] private SpringJoint _jp; /// <summary> Get a SpringJoint Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public SpringJoint JP { get { return _jp ?? (_jp = _GOAC<SpringJoint>());} }

    [NonSerialized] private FixedJoint _jf; /// <summary> Get a FixedJoint Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public FixedJoint JF { get { return _jf ?? (_jf = _GOAC<FixedJoint>());} }

    [NonSerialized] private ConfigurableJoint _jg; /// <summary> Get a ConfigurableJoint Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public ConfigurableJoint JG { get { return _jg ?? (_jg = _GOAC<ConfigurableJoint>());} }

 #endif
    
#if EX_COLLIDERS2D
    [NonSerialized] private BoxCollider2D _xb2d; /// <summary> Get a BoxCollider2D Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public BoxCollider2D XB2D { get { return _xb2d ?? (_xb2d = _GOAC<BoxCollider2D>());} }
    
    [NonSerialized] private CircleCollider2D _xc2d; /// <summary> Get a CircleCollider2D Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public CircleCollider2D XC2D { get { return _xc2d ?? (_xc2d = _GOAC<CircleCollider2D>());} }
    
    [NonSerialized] private PolygonCollider2D _xp2d; /// <summary> Get a PolygonCollider2D Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public PolygonCollider2D XP2D { get { return _xp2d ?? (_xp2d = _GOAC<PolygonCollider2D>());} }
    
    [NonSerialized] private EdgeCollider2D _xe3d; /// <summary> Get a EdgeCollider2D Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public EdgeCollider2D XE2D { get { return _xe3d ?? (_xe3d = _GOAC<EdgeCollider2D>());} }
 #endif
 
 #if EX_JOINTS2D
    [NonSerialized] private AnchoredJoint2D _ja2d; /// <summary> Get a AnchoredJoint2D Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public AnchoredJoint2D JA2D { get { return _ja2d ?? (_ja2d = _GOAC<AnchoredJoint2D>());} }
   
    [NonSerialized] private HingeJoint2D _jh2d; /// <summary> Get a HingeJoint2D Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public HingeJoint2D JH2D { get { return _jh2d ?? (_jh2d = _GOAC<HingeJoint2D>());} }
   
    [NonSerialized] private SliderJoint2D _js2d; /// <summary> Get a SliderJoint2D Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public SliderJoint2D JS2D { get { return _js2d ?? (_js2d = _GOAC<SliderJoint2D>());} }
   
    [NonSerialized] private SpringJoint2D _jp2d; /// <summary> Get a SpringJoint2D Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public SpringJoint2D JP2D { get { return _jp2d ?? (_jp2d = _GOAC<SpringJoint2D>());} }
   
    [NonSerialized] private WheelJoint2D _jw2d; /// <summary> Get a WheelJoint2D Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public WheelJoint2D JW2D { get { return _jw2d ?? (_jw2d = _GOAC<WheelJoint2D>());} }
  #endif
   
#if EX_UI
    [NonSerialized] private RectTransform _rt; /// <summary> Get a RectTransform Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public RectTransform RT { get { return _rt ?? (_rt = _GOAC<RectTransform>());} }

    [NonSerialized] private Text _tx; /// <summary> Get a Text Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Text TX { get { return _tx ?? (_tx = _GOAC<Text>());} }

    [NonSerialized] private Image _im; /// <summary> Get a Image Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Image IM { get { return _im ?? (_im = _GOAC<Image>());} }

    [NonSerialized] private LayoutElement _le; /// <summary> Get a LayoutElement Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public LayoutElement LE { get { return _le ?? (_le = _GOAC<LayoutElement>());} }
 #endif
    
#if EX_UI_VISUAL
    [NonSerialized] private Canvas _cv; /// <summary> Get a Canvas Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Canvas CV { get { return _cv ?? (_cv = _GOAC<Canvas>());} }

    [NonSerialized] private RawImage _ir; /// <summary> Get a RawImage Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public RawImage IR { get { return _ir ?? (_ir = _GOAC<RawImage>());} }

    [NonSerialized] private Mask _mk; /// <summary> Get a Mask Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Mask MK { get { return _mk ?? (_mk = _GOAC<Mask>());} }

    [NonSerialized] private RectMask2D _mk2d; /// <summary> Get a RectMask2D Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public RectMask2D MK2D { get { return _mk2d ?? (_mk2d = _GOAC<RectMask2D>());} }
 #endif
 
 #if EX_UI_INTERACTION
    [NonSerialized] private Button _bt; /// <summary> Get a Button Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Button BT { get { return _bt ?? (_bt = _GOAC<Button>());} }

    [NonSerialized] private Toggle _tg; /// <summary> Get a Toggle Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Toggle TG { get { return _tg ?? (_tg = _GOAC<Toggle>());} }

    [NonSerialized] private Slider _sl; /// <summary> Get a Slider Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Slider SL { get { return _sl ?? (_sl = _GOAC<Slider>());} }

    [NonSerialized] private Scrollbar _sb; /// <summary> Get a Scrollbar Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Scrollbar SB { get { return _sb ?? (_sb = _GOAC<Scrollbar>());} }
 
    [NonSerialized] private Dropdown _dd; /// <summary> Get a Dropdown Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public Dropdown DD { get { return _dd ?? (_dd = _GOAC<Dropdown>());} }

    [NonSerialized] private InputField _if; /// <summary> Get a InputField Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public InputField IF { get { return _if ?? (_if = _GOAC<InputField>());} }

    [NonSerialized] private ScrollRect _sr; /// <summary> Get a ScrollRect Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public ScrollRect SR { get { return _sr ?? (_sr = _GOAC<ScrollRect>());} }
#endif

#if EX_UI_LAYOUT
    [NonSerialized] private ContentSizeFitter _csf; /// <summary> Get a ContentSizeFitter Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public ContentSizeFitter CSF { get { return _csf ?? (_csf = _GOAC<ContentSizeFitter>());} }

    [NonSerialized] private AspectRatioFitter _arf; /// <summary> Get a AspectRatioFitter Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public AspectRatioFitter ARF { get { return _arf ?? (_arf = _GOAC<AspectRatioFitter>());} }

    [NonSerialized] private HorizontalLayoutGroup _hlg; /// <summary> Get a HorizontalLayoutGroup Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public HorizontalLayoutGroup HLG { get { return _hlg ?? (_hlg = _GOAC<HorizontalLayoutGroup>());} }

    [NonSerialized] private VerticalLayoutGroup _vlg; /// <summary> Get a VerticalLayoutGroup Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public VerticalLayoutGroup VLG { get { return _vlg ?? (_vlg = _GOAC<VerticalLayoutGroup>());} }

    [NonSerialized] private GridLayoutGroup _glg; /// <summary> Get a GridLayoutGroup Component </summary>
    [DebuggerBrowsable(_NEV), EditorBrowsableAttribute(_ADV)]
    public GridLayoutGroup GLG { get { return _glg ?? (_glg = _GOAC<GridLayoutGroup>());} }

#endif

    public Coroutine StartCoroutine (float time, Action<float> action, bool useRealtime = false) {
        return StartCoroutine(useRealtime ? _GetReltimeWrapper(time, action) : _GetWrapper(time, action));
    }
    
     public Coroutine Invoke (Action<int> action, float time = 0f, float repeatRate = 0f, bool useRealtime = false) {
        return StartCoroutine(useRealtime ? _GetRealtimeInvokeWrapper(action, time, repeatRate) : _GetInvokeWrapper(action, time, repeatRate));
    }
    
    static IEnumerator _GetInvokeWrapper (Action<int> action, float time, float repeatRate) {
        if (repeatRate < .02f)
            repeatRate = time;
        float now =  Time.time;
        int iterator = 0;
        int times = time < .02f ? 1 : Mathf.FloorToInt(time / repeatRate);
        while (iterator < times)
        {
            yield return new WaitForSeconds(repeatRate);
            action(iterator++);
        }
    }
    
     static IEnumerator _GetRealtimeInvokeWrapper (Action<int> action, float time, float repeatRate) {
        if (repeatRate < .02f)
            repeatRate = time;
        float now = Time.unscaledTime;
        int iterator = 0;
        while (Time.unscaledTime - now < time)
        {
            yield return new WaitForSecondsRealtime(repeatRate);
            action(iterator++);
        }
    }
    static IEnumerator _GetWrapper (float time, Action<float> action) {
        float now = Time.time;
        while (Time.time - now < time)
        {
            action((Time.time - now) / time);
            yield return null;
        }
        action(1);
    }
    
    static IEnumerator _GetReltimeWrapper (float time, Action<float> action) {
        float now = Time.unscaledTime;
        while (Time.unscaledTime - now < time)
        {
            action((Time.unscaledTime - now) / time);
            yield return null;
        }
        action(1);
    }
}
