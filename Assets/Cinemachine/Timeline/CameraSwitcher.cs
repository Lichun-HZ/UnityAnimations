using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CameraSwitchPlayable : PlayableBehaviour
{
    public Camera camera;
    private bool m_WasEnabled;

    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);
        if (camera != null)
        {
            m_WasEnabled = camera.enabled;
        }
    }

    public override void OnGraphStop(Playable playable)
    {
        base.OnGraphStop(playable);
        if (camera != null)
        {
            camera.enabled = m_WasEnabled;
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (camera != null)
        {
            camera.enabled = false;
        }
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (camera != null)
        {
            camera.enabled = true;
        }
    }

}

[System.Serializable]
public class CameraSwitcher : PlayableAsset
{
    public ExposedReference<Camera> camera;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CameraSwitchPlayable>.Create(graph);
        playable.GetBehaviour().camera = camera.Resolve(graph.GetResolver());
        return playable;
    }
}

