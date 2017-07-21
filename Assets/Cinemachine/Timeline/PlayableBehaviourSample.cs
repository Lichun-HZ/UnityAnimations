using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.Timeline;

public class BlenderPlayableBehaviour : PlayableBehaviour
{
    public AnimationMixerPlayable mixerPlayable;

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        float blend = Mathf.PingPong((float)playable.GetTime(), 1.0f);

        mixerPlayable.SetInputWeight(0, blend);
        mixerPlayable.SetInputWeight(1, 1.0f - blend);

        base.PrepareFrame(playable, info);
    }
}

public class PlayableBehaviourSample : MonoBehaviour
{
    public PlayableGraph m_Graph;
    public AnimationClip clipA;
    public AnimationClip clipB;

    // Use this for initialization
    void Start()
    {
        // Create the PlayableGraph.
        m_Graph = PlayableGraph.Create();

        // Add an AnimationPlayableOutput to the graph.
        var animOutput = AnimationPlayableOutput.Create(m_Graph, "AnimationOutput", GetComponent<Animator>());

        // Add an AnimationMixerPlayable to the graph.
        var mixerPlayable = AnimationMixerPlayable.Create(m_Graph, 2, false);

        // Add two AnimationClipPlayable to the graph.
        var clipPlayableA = AnimationClipPlayable.Create(m_Graph, clipA);
        var clipPlayableB = AnimationClipPlayable.Create(m_Graph, clipB);

        // Add a custom PlayableBehaviour to the graph.
        // This behavior will change the weights of the mixer dynamically.
        var blenderPlayable = ScriptPlayable<BlenderPlayableBehaviour>.Create(m_Graph, 1);
        blenderPlayable.GetBehaviour().mixerPlayable = mixerPlayable;
        blenderPlayable.SetInputWeight(0, 1.0f);

        // Create the topology, connect the AnimationClipPlayable to the
        // AnimationMixerPlayable.  Also add the BlenderPlayableBehaviour.
        m_Graph.Connect(clipPlayableA, 0, mixerPlayable, 0);
        m_Graph.Connect(clipPlayableB, 0, mixerPlayable, 1);
        m_Graph.Connect(mixerPlayable, 0, blenderPlayable, 0);

        // Use the AnimationMixerPlayable as the source for the AnimationPlayableOutput.
        animOutput.SetSourcePlayable(blenderPlayable);

        // Play the graph.
        m_Graph.Play();
    }

    private void OnDestroy()
    {
        // Destroy the graph once done with it.
        m_Graph.Destroy();
    }
}
