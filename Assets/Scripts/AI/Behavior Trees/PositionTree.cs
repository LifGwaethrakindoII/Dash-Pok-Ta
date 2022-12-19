using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoidlessUtilities;
using DashPokTa;

public class PositionTree : BasePlayerBehaviorTree
{
	private PlayerModel referencePlayer; 		/// <summary>GameObject's Reference Player.</summary>
	private Vector3 movePosition; 				/// <summary>Position where the player will move.</summary>

#region UnityMethods:
	/// <summary>Comence Behavior Tree Ticker.</summary>
	void OnEnable()
	{
		if(rootSelector != null) treeTicker = new Behavior(this, RunTree());
	}

	/// <summary>End Behavior Tree Ticker.</summary>
	void OnDisable()
	{
		if(treeTicker != null) treeTicker.EndBehavior(); //Turn off the Positin Tree.
	}

	/// <summary>Movement on Update.</summary>
	void Update()
	{
		if(actionRoot != null) actionRoot.Tick();
		if(player.animator != null) player.UpdateAnimatorControllerParameters();
	}
#endregion

#region TreeInitializations:
	/// <summary>Initializes Support Defender BehaviorTree.</summary>
	/// <returns>Support Defender Tree Setted.</summary>
	public override SelectorNode InitializeSupportDefenderTree()
	{
		if(player.team is LocalTeamManager)
		{//If Player is from Local Team.
			supportDefenderRoot = 
			new SelectorNode
			(//Root Selector
#region OffensiveParadigm:
				new SequenceNode
				(//Offensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Offensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportAttacker),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region DefensiveParadigm:
				new SequenceNode
				(//Defensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Defensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportAttacker),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSuppoert.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalSuppoert.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalSuppoert.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region PassiveParadigm:
				new SequenceNode
				(//Passive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Passive),
					new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
					new ActionTask<TreeNode.States>(SetMovePosition),
					new SelectorNode
					(//Movement Action Selector.
						new SequenceNode
						(
							new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
							new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
						),
						new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
					)
				)
#endregion
			);
		}
		else if(player.team is VisitorTeamManager)
		{//If Player is from Visitor Team.
			supportDefenderRoot = 
			new SelectorNode
			(//Root Selector
#region OffensiveParadigm:
				new SequenceNode
				(//Offensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Offensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportAttacker),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region DefensiveParadigm:
				new SequenceNode
				(//Defensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Defensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportAttacker),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSuppoert.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorSuppoert.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorSuppoert.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region PassiveParadigm:
				new SequenceNode
				(//Passive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Passive),
					new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
					new ActionTask<TreeNode.States>(SetMovePosition),
					new SelectorNode
					(//Movement Action Selector.
						new SequenceNode
						(
							new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
							new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
						),
						new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
					)
				)
#endregion
			);
		}

		return supportDefenderRoot; 
	}

	/// <summary>Initializes Main Defender BehaviorTree.</summary>
	/// <returns>Main Defender Tree Setted.</summary>
	public override SelectorNode InitializeMaintDefenderTree()
	{
		if(player.team is LocalTeamManager)
		{//If Player is from Local Team.
			mainDefenderRoot = 
			new SelectorNode
			(//Root Selector
#region OffensiveParadigm:
				new SequenceNode
				(//Offensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Offensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportDefender),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle Zone.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Priority LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region DefensiveParadigm:
				new SequenceNode
				(//Defensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Defensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportDefender),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle Zone.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Priority LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region PassiveParadigm:
				new SequenceNode
				(//Passive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Passive),
					new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
					new ActionTask<TreeNode.States>(SetMovePosition),
					new SelectorNode
					(//Movement Action Selector.
						new SequenceNode
						(
							new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
							new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
						),
						new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
					)
				)
#endregion
			);
		}
		else if(player.team is VisitorTeamManager)
		{//If Player is from Visitor Team.
			mainDefenderRoot = 
			new SelectorNode
			(//Root Selector
#region OffensiveParadigm:
				new SequenceNode
				(//Offensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Offensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportDefender),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle Zone.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Priority VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region DefensiveParadigm:
				new SequenceNode
				(//Defensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Defensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportDefender),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle Zone.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Priority VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Priority LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Priority LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region PassiveParadigm:
				new SequenceNode
				(//Passive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Passive),
					new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
					new ActionTask<TreeNode.States>(SetMovePosition),
					new SelectorNode
					(//Movement Action Selector.
						new SequenceNode
						(
							new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
							new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
						),
						new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
					)
				)
#endregion
			);
		}

		return mainDefenderRoot;
	}

	/// <summary>Initializes Support Attacker BehaviorTree.</summary>
	/// <returns>Support Attacker Tree Setted.</summary>
	public override SelectorNode InitializeSupportAttackerTree()
	{
		if(player.team is LocalTeamManager)
		{//If Player is from Local Team.
			supportAttackerRoot = 
			new SelectorNode
			(//Root Selector
#region OffensiveParadigm:
				new SequenceNode
				(//Offensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Offensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportAttacker),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region DefensiveParadigm:
				new SequenceNode
				(//Defensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Defensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportAttacker),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region PassiveParadigm:
				new SequenceNode
				(//Passive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Passive),
					new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
					new ActionTask<TreeNode.States>(SetMovePosition),
					new SelectorNode
					(//Movement Action Selector.
						new SequenceNode
						(
							new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
							new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
						),
						new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
					)
				)
#endregion
			);
		}
		else if(player.team is VisitorTeamManager)
		{//If Player is from Visitor Team.
			supportAttackerRoot = 
			new SelectorNode
			(//Root Selector
#region OffensiveParadigm:
				new SequenceNode
				(//Offensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Offensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportAttacker),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region DefensiveParadigm:
				new SequenceNode
				(//Defensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Defensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportAttacker),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region PassiveParadigm:
				new SequenceNode
				(//Passive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Passive),
					new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
					new ActionTask<TreeNode.States>(SetMovePosition),
					new SelectorNode
					(//Movement Action Selector.
						new SequenceNode
						(
							new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
							new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
						),
						new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
					)
				)
#endregion
			);
		}

		return supportAttackerRoot;
	}

	/// <summary>Initializes Main Attacker BehaviorTree.</summary>
	/// <returns>Main Attacker Tree Setted.</summary>
	public override SelectorNode InitializeMainAttackerTree()
	{
		if(player.team is LocalTeamManager)
		{//If Player is from Local Team.
			mainAttackerRoot = 
			new SelectorNode
			(//Root Selector
#region OffensiveParadigm:
				new SequenceNode
				(//Offensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Offensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportAttacker),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region DefensiveParadigm:
				new SequenceNode
				(//Defensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Defensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportAttacker),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region PassiveParadigm:
				new SequenceNode
				(//Passive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Passive),
					new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
					new ActionTask<TreeNode.States>(SetMovePosition),
					new SelectorNode
					(//Movement Action Selector.
						new SequenceNode
						(
							new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
							new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
						),
						new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
					)
				)
#endregion
			);
		}
		else if(player.team is VisitorTeamManager)
		{//If Player is from Visitor Team.
			supportDefenderRoot = 
			new SelectorNode
			(//Root Selector
#region OffensiveParadigm:
				new SequenceNode
				(//Offensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Offensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportAttacker),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region DefensiveParadigm:
				new SequenceNode
				(//Defensive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Defensive),
					new ActionTask<PlayerRoles>(SetReferencePlayer, PlayerRoles.SupportAttacker),
					new SelectorNode
					(//Zones Selector.
						new SequenceNode
						(//Reference Player on LocalMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on Center.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.Center),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on Center.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.Center),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On Center.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorWing.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorWing),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorWing.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorWing.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on LocalSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.LocalSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on LocalSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.LocalSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On LocalSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorSupport.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorSupport),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorSupport.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorSupport.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorMiddle.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorMiddle),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorMiddle.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.Center),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorMiddle.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.LocalWing),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						),
						new SequenceNode
						(//Reference Player on VisitorGoal.
							new ConditionTask<Playground.PlaygroundZones>(IsTargetOnZone, Playground.PlaygroundZones.VisitorGoal),
							new SelectorNode
							(//Zone Ball Is Selector.
								new SequenceNode
								( //Is Ball on VisitorGoal.
									new ConditionTask<Playground.PlaygroundZones>(IsBallOnZone, Playground.PlaygroundZones.VisitorGoal),
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorMiddle),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								),
								new SequenceNode
								(//Ball not On VisitorGoal.
									new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorSupport),
									new ActionTask<TreeNode.States>(SetMovePosition),
									new SelectorNode
									(//Movement Action Selector.
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
										),
										new SequenceNode
										(
											new ConditionTask<TreeNode.States>(HasStamina),
											new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
										),
										new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
									)		
								)
							)
						)
					)
				),
#endregion
#region PassiveParadigm:
				new SequenceNode
				(//Passive Paradigm
					new ConditionTask<TeamManager.Paradigms>(TeamHasParadigm, TeamManager.Paradigms.Passive),
					new ActionTask<Playground.PlaygroundZones>(SetPlaygroundZone, Playground.PlaygroundZones.VisitorWing),
					new ActionTask<TreeNode.States>(SetMovePosition),
					new SelectorNode
					(//Movement Action Selector.
						new SequenceNode
						(
							new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
							new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
						),
						new ActionTask<MovementActions>(SetMovementAction, MovementActions.Run)
					)
				)
#endregion
			);
		}

		return mainAttackerRoot;
	}

	/// <summary>Initializes Action BehaviorTree.</summary>
	/// <returns>Action Tree Setted.</summary>
	public override SelectorNode InitializeActionTree()
	{
		actionRoot
		= new SelectorNode
		(//Main Parent Node.
			new SequenceNode
			(//Run Movement Action.
				new ConditionTask<MovementActions>(HasMovementAction, MovementActions.Run),
				new SelectorNode
				(//Run or Walk Selector
					new SequenceNode
					( //If close, deaccelerate.
						new ConditionTask<TreeNode.States>(IsCloseToDesignatedPosition),
						new ActionTask<MovementActions>(SetMovementAction, MovementActions.Wait)
					),
					new SequenceNode
					(//Run Sequence.
						new ConditionTask<TreeNode.States>(HasStamina),
						new ActionTask<TreeNode.States>(RunToTarget)
					),
					new ActionTask<MovementActions>(SetMovementAction, MovementActions.Walk)
				)
			),
			new SequenceNode
			(//Walk Movement Action.
				new ConditionTask<MovementActions>(HasMovementAction, MovementActions.Walk),
				new ActionTask<TreeNode.States>(Deaccelerate),
				new ActionTask<TreeNode.States>(WalkToTarget)
			),
			new SequenceNode
			(//Wait Movement Action.
				new ConditionTask<MovementActions>(HasMovementAction, MovementActions.Wait),
				new ActionTask<TreeNode.States>(Deaccelerate),
				new ActionTask<TreeNode.States>(Wait)
			)
		);

		return actionRoot;
	}
#endregion


#region ConditionLeafs:
	/// <summary>Checks if Player has Stamina.</summary>
	/// <returns>Sta eof the Condition.</summary>
	private TreeNode.States HasStamina()
	{
		if(player.stamina > player.runStaminaSpend) return TreeNode.States.Success;
		else return TreeNode.States.Failure;
	}

	/// <summary>Checks the side of the Player's Team.</summary>
	/// <param name="_team">Player's Team.</summary>
	/// <returns>Success if the Player is of the requested Team.</summary>
	private TreeNode.States IsOnTeam(TeamManager _team)
	{
		if(player.team == null) return TreeNode.States.Error;
		else if(player.team.GetType() == _team.GetType()) return TreeNode.States.Success;
		else return TreeNode.States.Failure;
	}

	/// <summary>Checks current Movement Action assigned.</summary>
	/// <param name="_movementAction">Requested Movement Action.</summary>
	/// <returns>Success if has requested Movement Action.</summary>
	private TreeNode.States HasMovementAction(MovementActions _movementAction)
	{
		if(movementAction == _movementAction) return TreeNode.States.Success;
		else return TreeNode.States.Failure;
	}

	/// <summary>Checks if the Player has the requested role.</summary>
	/// <param name="_rol">Requested Role.</summary>
	/// <returns>State of the Condition.</summary>
	private TreeNode.States HaveRol(PlayerRoles _role)
	{
		if(player.role == _role) return TreeNode.States.Success;
		else return TreeNode.States.Failure;

	}

	/// <summary>Checks if the Player is on Playground Zone.</summary>
	/// <param name="_zone">Requested Zone.</summary>
	/// <returns>State of the Condition.</summary>
	private TreeNode.States IsTargetOnZone(Playground.PlaygroundZones _zone)
	{
		/*IPlaygroundZoneCheck playgroundCheck = player.directionReference.GetComponent<IPlaygroundZoneCheck>();

		if(playgroundCheck == null || player.directionReference == null) return TreeNode.States.Error;*/

		Debug.Log(player.role.ToString() + " is on Zone: " + player.currentZone.ToString());

		if(referencePlayer.currentZone == _zone) return TreeNode.States.Success;
		else return TreeNode.States.Failure;
	}


	/// <summary>Checks if the Player's Team has requested Paradigm.</summary>
	/// <param name="_paradigm">Requested Paradigm.</summary>
	/// <returns>State of the Condition.</summary>
	private TreeNode.States TeamHasParadigm(TeamManager.Paradigms _paradigm)
	{
		Debug.Log("Paradigm: " + _paradigm.ToString() + ". Team's Paradigm: " + player.team.paradigm.ToString());
		if(player.team.paradigm == _paradigm) return TreeNode.States.Success;
		else return TreeNode.States.Failure;
	}

	/// <summary>Checks if the Ball is on requested Playground Zone.</summary>
	/// <param name="_zone">Requested Zone.</summary>
	/// <returns>State of the Condition.</summary>
	private TreeNode.States IsBallOnZone(Playground.PlaygroundZones _zone)
	{
		Debug.Log(_zone.ToString());
		if(BallModel.Instance.currentZone == _zone) return TreeNode.States.Success;
		else return TreeNode.States.Failure;
	}

	/// <summary>Checks if Player is close to deisgnated position.</summary>
	/// <returns>State of the Condition.</summary>
	private TreeNode.States IsCloseToDesignatedPosition()
	{
		if(transform.position.GetDirectionTowards(movePosition).SetY(0f).sqrMagnitude < 3.75f) return TreeNode.States.Success;
		else return TreeNode.States.Failure;		
	}
#endregion

#region ActionLeafs:
	/// <summary>Sets Reference Player.</summary>
	/// <param name="_referencePlayer">New Reference Player.</summary>
	/// <returns>State of the Action.</summary>
	private TreeNode.States SetReferencePlayer(PlayerRoles _referencePlayer)
	{
		referencePlayer = player.team.teamPlayers[_referencePlayer];
		if(referencePlayer != null) return TreeNode.States.Success;
		else return TreeNode.States.Failure;
	}

	/// <summary>Sets Playground Zone to Move.</summary>
	/// <param name="_playgroundZone">New Playground Zone.</summary>
	/// <returns>State of the Action.</summary>
	private TreeNode.States SetPlaygroundZone(Playground.PlaygroundZones _playgroundZone)
	{
		player.directionReference = null;
		player.directionReference = Playground.Instance.zones[_playgroundZone].transform;
		if(player.directionReference != null) return TreeNode.States.Success;
		else return TreeNode.States.Failure;
	}

	/// <summary>Sets Current Movement Action.</summary>
	/// <param name="_movementAction">Requested Movement Action.</summary>
	/// <returns>State of the Action.</summary>
	private TreeNode.States SetMovementAction(MovementActions _movementAction)
	{
		movementAction = _movementAction;

		switch(movementAction)
		{
			case MovementActions.Wait:
			player.jumping = false;
			player.running = false;
			player.dashing = false;
			player.axisesBelowZero = false;
			break;

			case MovementActions.Walk:
			player.axisesBelowZero = true;
			player.jumping = false;
			player.running = false;
			player.dashing = false;
			break;

			case MovementActions.Run:
			player.axisesBelowZero = true;
			player.running = true;
			player.jumping = false;
			player.dashing = false;
			break;
		}

		if(movementAction == _movementAction) return TreeNode.States.Success;
		else return TreeNode.States.Failure;
	}

	/// <summary>Deaccelerates player, if its acceleration reaches the minimum, it puts the Player on Idle State.</summary>
	/// <returns>State of the Action.</summary>
	private TreeNode.States Deaccelerate()
	{
		player.Deaccelerate(PlayerModel.StaminaTypes.Run);
		player.RecoverStamina();
		return TreeNode.States.Success;
	}

	private TreeNode.States Wait()
	{
		player.Wait();
		return TreeNode.States.Success;
	}

	private TreeNode.States SetMovePosition()
	{
		movePosition = VoidlessMath.GetMiddlePointBetween(player.directionReference.position, BallModel.Instance.transform.position).SetY(0f);
		return TreeNode.States.Success;
	}

	/// <summary>Mover player towards new position Player.</summary>
	/// <returns>State of the Action.</summary>
	private TreeNode.States RunToTarget()
	{
		//1. Get Direction Vector between player and target.
		//2. Normalize the Direction Vector.
		//3. Round the Vector Components, to get a Vector of [0-1] components, getting so a normalized direction to turn around.
		//4. Calculate Middle Point between Rounded Point and Ball.
		player.FaceDirection(transform.position.GetDirectionTowards(movePosition).normalized.Round().SetY(0f));
		player.Run();

		return TreeNode.States.Success;
		//return TreeNode.States.Running;
	}

	/// <summary>Walk player towards new position Player.</summary>
	/// <returns>State of the Action.</summary>
	private TreeNode.States WalkToTarget()
	{
		//1. Get Direction Vector between player and target.
		//2. Normalize the Direction Vector.
		//3. Round the Vector Components, to get a Vector of [0-1] components, getting so a normalized direction to turn around.
		//4. Calculate Middle Point between Rounded Point and Ball.
		player.FaceDirection(transform.position.GetDirectionTowards(movePosition).normalized.Round().SetY(0f));
		player.Walk();

		return TreeNode.States.Success;
		//return TreeNode.States.Running;
	}
#endregion

	/// <summary>Ticks the Position Tree.</summary>
	public override IEnumerator RunTree()
	{
		while(true)
		{
			rootSelector.Tick();
			yield return new WaitForSeconds(player.reactionTime);
			Debug.LogWarning("Root State: " + rootSelector.state.ToString());
		}	
	}
}
