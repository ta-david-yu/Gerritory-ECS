using JCMG.EntitasRedux;

public interface IEntityCreationEventController
{
	void OnEntityCreated(IEntity entity);
	void OnComponentsAdded(IEntity entity);
	void Link(IEntity entity);
}
