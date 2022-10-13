using JCMG.EntitasRedux;

public interface IEntityCreationEventController
{
	void OnEntityCreated(Contexts contexts, IEntity entity);
	void OnComponentsAdded(Contexts contexts, IEntity entity);
	void Link(IEntity entity);
}
