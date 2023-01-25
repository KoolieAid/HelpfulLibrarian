using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tutorial
{
    /// <summary>
    /// Attach this to a <see cref="GameObject"/> to enable sequenced movement.
    /// <seealso cref="Sequence"/>
    /// </summary>
    [AddComponentMenu("Tutorial/Sequence Controller")]
    public class SequenceController : MonoBehaviour
    {
        private Queue<Sequence> queue = new();
        private Queue<Sequence> _internalCopy;

        private void Start()
        {
            _internalCopy = new Queue<Sequence>(queue);
            StartCoroutine(Loop());
        }

        private IEnumerator Loop()
        {
            foreach (var sequence in queue)
            {
                sequence.Execute(gameObject);
                yield return new WaitUntil(sequence.IsDone);
            }
        }

        public bool Repeat()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// <para>
        /// Adds a sequence to the queue of the controller.
        /// Instantiate a derived class of a Sequence using the `new` keyword
        /// </para>
        /// </summary>
        /// <example>
        /// <code>
        /// // Waits 5 seconds
        /// controller.AddSequence(new WaitSequence(controller, 5.0f)
        ///     .AddSequence(new WaitSequence(controller, 2.0f);
        /// </code>
        /// </example>
        /// <param name="s">Use this for chaining Sequences</param>
        /// <returns></returns>
        /// <seealso cref="WaitSequence"/>
        /// <remarks>
        /// It is recommended to use this in Awake instead of Start
        /// </remarks>
        public SequenceController AddSequence(Sequence s)
        {
            queue.Enqueue(s);
            return this;
        }
    }

    /// <summary>
    /// Base class for sequences
    /// <list type="Bullet">
    /// <item>
    /// <see cref="WaitSequence"/>
    /// </item>
    /// <item>
    /// <see cref="MoveSequence"/>
    /// </item>
    /// <item>
    /// <see cref="ToolTipSequence"/>
    /// </item>
    /// <item>
    /// <see cref="CustomSequence"/>
    /// </item>
    /// <item>
    /// <see cref="FocusSequence"/> - Not done
    /// </item>
    /// </list>
    /// </summary>
    public abstract class Sequence
    {
        protected bool isDone;
        protected readonly SequenceController _controller;

        /// <summary>
        /// Constructor for the sequence.
        /// </summary>
        /// <param name="controller">Controller for getting</param>
        protected Sequence(SequenceController controller)
        {
            _controller = controller;
        }

        /// <summary>
        /// Checks if the sequence is done
        /// </summary>
        /// <returns>
        /// True - Sequence is done.
        /// False - Sequence is not done.
        /// </returns>
        /// <example>
        /// <code>
        /// IEnumerator Example()
        /// {
        ///     yield return new WaitUntil(sequence.IsDone());
        /// }
        /// </code>
        /// </example>
        public bool IsDone()
        {
            return isDone;
        }

        /// <summary>
        /// <para>
        /// Use this method if you want to manually execute the sequence.
        /// By default it is done automatically by the
        /// <see cref="SequenceController"/>
        /// </para>
        /// </summary>
        /// <param name="o">GameObject in which the sequence acts upon.</param>
        public abstract void Execute(GameObject o);
    }

    public class WaitSequence : Sequence
    {
        private readonly float _time;

        /// <summary>
        /// Waits a set amount of time
        /// </summary>
        /// <param name="time">Time to wait in seconds</param>
        public WaitSequence(SequenceController controller, float time) : base(controller)
        {
            _time = time;
        }

        public override void Execute(GameObject o)
        {
            _controller.StartCoroutine(_wait());
        }

        private IEnumerator _wait()
        {
            yield return new WaitForSeconds(_time);
            isDone = true;
        }
    }

    /*
     * Move to a designated spot with a designated speed
     */
    public class MoveSequence : Sequence
    {
        private readonly Vector2 _destination;
        private readonly float _speed;
        private readonly float _tolerance;

        /// <summary>
        /// Sequence to move the object into a designated spot
        /// </summary>
        /// <param name="controller">Controller for referencing</param>
        /// <param name="destination">Designated spot</param>
        /// <param name="speed">Speed in which the object is going to</param>
        /// <param name="tolerance">Distance between the object and destination in which the objects stops</param>
        public MoveSequence(SequenceController controller, Vector2 destination, float speed, float tolerance = 0.1f) :
            base(controller)
        {
            _destination = destination;
            _speed = speed;
            _tolerance = tolerance;
        }

        public override void Execute(GameObject o)
        {
            _controller.StartCoroutine(_loop(o));
        }

        private IEnumerator _loop(GameObject o)
        {
            while (Vector2.Distance(o.transform.position, _destination) > _tolerance)
            {
                yield return new WaitForEndOfFrame();
                o.transform.position = Vector2.MoveTowards(o.transform.position, _destination, _speed * Time.deltaTime);
            }

            isDone = true;
        }
    }
    
    public class MoveSequenceCanvas : Sequence
    {
        private readonly Vector2 _destination;
        private readonly float _speed;
        private readonly float _tolerance;
        private RectTransform _rectTransform;

        /// <summary>
        /// Sequence to move the object into a designated spot
        /// </summary>
        /// <param name="controller">Controller for referencing</param>
        /// <param name="destination">Designated spot</param>
        /// <param name="speed">Speed in which the object is going to</param>
        /// <param name="tolerance">Distance between the object and destination in which the objects stops</param>
        /// <param name="rectTransform">RectTransform component of the object in question</param>
        /// <remarks>
        /// Only used for objects that are in a canvas
        /// </remarks>
        public MoveSequenceCanvas(SequenceController controller, Vector2 destination, float speed, RectTransform rectTransform, float tolerance = 0.1f) :
            base(controller)
        {
            _destination = destination;
            _speed = speed;
            _tolerance = tolerance;
            _rectTransform = rectTransform;
        }

        public override void Execute(GameObject o)
        {
            _controller.StartCoroutine(_loop(o));
        }

        private IEnumerator _loop(GameObject o)
        {
            while (Vector2.Distance(_rectTransform.anchoredPosition, _destination) > _tolerance)
            {
                yield return new WaitForEndOfFrame();
                _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, _destination, _speed * Time.deltaTime);
            }

            isDone = true;
        }
    }
    
    public class ToolTipSequence : Sequence
    {
        protected ToolTipAdapter _toolTip;
        private string _text;
        private bool _turnOn;

        /// <summary>
        /// Shows a tooltip with a set prefab.
        /// </summary>
        /// <param name="controller">Controller For reference</param>
        /// <param name="toolTip">The ToolTipAdapter game object to toggle states</param>
        /// <param name="text">Text to Display</param>
        /// <param name="turnOn">Whether the tooltip will turn on or not</param>
        /// <remarks>
        /// The other constructor is the same except the offset is default to zero.
        /// The tooltip prefab needs to have <see cref="ToolTipAdapter"/> attached to it.
        /// </remarks>
        public ToolTipSequence(SequenceController controller, ToolTipAdapter toolTip, string text, bool turnOn = true) : base(controller)
        {
            _toolTip = toolTip;
            _text = text;
            _turnOn = turnOn;
        }

        /// <summary>
        /// Turns off the provided tool tip
        /// </summary>
        /// <param name="controller">For referencing</param>
        /// <param name="toolTip">Tool tip to turn off</param>
        public ToolTipSequence(SequenceController controller, ToolTipAdapter toolTip) : this(controller, toolTip, "", false)
        {
        }

        public override void Execute(GameObject o)
        {
            _toolTip.SetText(_text);
            _toolTip.gameObject.SetActive(_turnOn);
            isDone = true;
        }
    }

    public class TwoToolTipSequence : ToolTipSequence
    {
        private string _text2;
        public TwoToolTipSequence(SequenceController controller, Text2ToolTipAdapter adapter, string t, string t2,
            bool turnOn = true)
            : base(controller, adapter, t, turnOn)
        {
            _text2 = t2;
        }

        public TwoToolTipSequence(SequenceController controller, Text2ToolTipAdapter adapter)
            : this(controller, adapter, "", "", false)
        {
        }

        public override void Execute(GameObject o)
        {
            Text2ToolTipAdapter t;
            try
            {
                t = (Text2ToolTipAdapter)_toolTip;
            }
            catch (Exception)
            {
                Debug.LogError($"Provided tool tip is not compatible with this sequence");
                return;
            }
            
            t.SetSecondText(_text2);
            base.Execute(o);
        }
    }

    public class CustomSequence : Sequence
    {
        private Action<CustomSequence, GameObject> _action;

        /// <summary>
        /// Allows the developer to make custom Sequences without deriving from the base class. 
        /// </summary>
        /// <param name="controller">Controller for referencing.</param>
        /// <param name="action">Code to be executed automatically.</param>
        public CustomSequence(SequenceController controller, Action<CustomSequence, GameObject> action) : base(controller)
        {
            _action = action;
        }

        /// <summary>
        /// Allows the developer to make custom Sequences without deriving from the base class.
        /// Using this constructor leaves the execution empty.
        /// </summary>
        /// <example>
        /// Using this constructor without actions
        /// <code>
        /// controller.AddSequence(new CustomSequence(controller)
        ///             .SetAction(o => {
        ///                 o.transform.position = new Vector(0,0,0);
        ///             }));
        /// </code>
        /// </example>
        /// <param name="controller">For referencing.</param>
        /// <seealso cref="Action{T}"/>
        public CustomSequence(SequenceController controller) : this(controller, null)
        {
        }

        /// <summary>
        /// Gets the controller for use in the custom code.
        /// </summary>
        /// <returns>The controller in question.</returns>
        public SequenceController GetController()
        {
            return _controller;
        }

        /// <summary>
        /// <para>
        /// Sets the action to be executed.
        /// Allows for chaining inside the SequenceController#AddSequence
        /// </para>
        /// </summary>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The <see cref="CustomSequence"/> for chaining purposes.</returns>
        public CustomSequence SetAction(Action<CustomSequence, GameObject> action)
        {
            _action = action;
            return this;
        }

        public override void Execute(GameObject o)
        {
            _action?.Invoke(this, o);
        }

        /// <summary>
        /// Change the status of the sequence to either finish it or revert to unfinished state
        /// </summary>
        /// <param name="newStatus">The new state</param>
        public void SetStatus(bool newStatus)
        {
            isDone = newStatus;
        }
    }
    
    public class FocusSequence : Sequence
    {
        private readonly float _duration;
        private readonly GameObject _target;
        
        /// <summary>
        /// Makes everything dark except for the object in focus.
        /// Not done.
        /// </summary>
        /// <param name="controller">Controller for referencing</param>
        /// <param name="target">Target to focus on.</param>
        /// <param name="duration">How long the focus is going to last.</param>
        public FocusSequence(SequenceController controller, GameObject target, float duration = 3.0f) : base(controller)
        {
            _duration = duration;
            _target = target;
        }

        public override void Execute(GameObject o)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Sequence that awaits a user input. Broadcast input using Toggle()
    /// </summary>
    /// <remarks>
    /// It is advisable to reuse this sequence when adding in the controller.
    /// </remarks>
    public class UserInputSequence : Sequence
    {
        public UserInputSequence(SequenceController controller) : base(controller)
        {
        }
        
        public override void Execute(GameObject o)
        {
        }

        private IEnumerator Framework()
        {
            isDone = true;
            yield return new WaitForEndOfFrame();
            isDone = false;
        }

        /// <summary>
        /// Toggles the internal boolean instantaneously
        /// </summary>
        /// <remarks>
        /// It is advisable to reuse the sequence.
        /// </remarks>
        public void Toggle()
        {
            _controller.StartCoroutine(Framework());
        }
    }

}