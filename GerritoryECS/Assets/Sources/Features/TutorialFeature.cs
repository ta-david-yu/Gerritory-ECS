using System.Collections;
using System.Collections.Generic;
using JCMG.EntitasRedux;

public class TutorialFeature : Feature
{
	public TutorialFeature(Contexts contexts) : base("Tutorial Feature")
	{
		Add(new HelloWorldSystem(contexts));
		Add(new DebugMessageSystem(contexts));
	}
}
