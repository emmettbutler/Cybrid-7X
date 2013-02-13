using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Graphics;
using System.Collections.Generic;

namespace realjam {
  public class Collider {

    public enum CollisionEntityType{
      Player,
      Cell,
    }

    public delegate Vector2 GetCenterDelegate();
    public delegate float GetRadiusDelegate();

    public struct CollisionEntry{
      public CollisionEntityType type;
      public GameEntity owner;
      public GetCenterDelegate center;
      public GetRadiusDelegate radius;
    }

    public List<CollisionEntry> collisionEntries;

    public Collider() {
      collisionEntries = new List<CollisionEntry>();
    }

    public void add(GameEntity instance){
      CollisionEntry entry = new CollisionEntry();
      entry.owner = instance;

      if(instance is Player){
        entry.type = CollisionEntityType.Player;
      } else if (instance is Cell){
        entry.type = CollisionEntityType.Cell;
      }

      entry.center = instance.GetCenter;
      entry.radius = instance.GetRadius;

      collisionEntries.Add(entry);
    }

    public void remove(GameEntity instance){
      CollisionEntry e = collisionEntries.Find(x => x.owner == instance);
      collisionEntries.Remove(e);
    }

    public void CollideWalls(Scene scene){
      Bounds2 bounds = scene.Camera.CalcBounds();
      for(int i = 0; i < collisionEntries.Count; ++i){
        CollisionEntry entry = collisionEntries[i];
        Vector2 center = entry.center();

        if(center.X < bounds.Min.X + entry.radius()){
          entry.owner.Position = new Vector2(bounds.Min.X + entry.radius(), entry.owner.Position.Y);
        } else if(center.X > bounds.Max.X - entry.radius()){
          entry.owner.Position = new Vector2(bounds.Max.X - entry.radius(), entry.owner.Position.Y);
        }

        if(center.Y < bounds.Min.Y + entry.radius()){
          entry.owner.Position = new Vector2(entry.owner.Position.X, bounds.Min.Y + entry.radius());
        } else if(center.Y > bounds.Max.Y - entry.radius()){
          entry.owner.Position = new Vector2(entry.owner.Position.X, bounds.Max.Y - entry.radius());
        }

        if(entry.owner.sprite != null){
          if(center.X < bounds.Min.X + entry.radius()){
            entry.owner.sprite.Position = new Vector2(bounds.Min.X + entry.radius(), entry.owner.sprite.Position.Y);
          } else if(center.X > bounds.Max.X - entry.radius()){
            entry.owner.sprite.Position = new Vector2(bounds.Max.X - entry.radius(), entry.owner.sprite.Position.Y);
          }

          if(center.Y < bounds.Min.Y + entry.radius()){
            entry.owner.sprite.Position = new Vector2(entry.owner.sprite.Position.X, bounds.Min.Y + entry.radius());
          } else if(center.Y > bounds.Max.Y - entry.radius()){
            entry.owner.sprite.Position = new Vector2(entry.owner.sprite.Position.X, bounds.Max.Y - entry.radius());
          }
        }
      }
    }

    public void Collide(){
      for(int i = 0; i < collisionEntries.Count; ++i){
        for(int j = 0; j < collisionEntries.Count; ++j){
          CollisionEntry collidee = collisionEntries[i];
          CollisionEntry collider = collisionEntries[j];
          if(collider.owner == collidee.owner){
            continue;
          }
          Vector2 collideeCenter = collidee.center();
          Vector2 colliderCenter = collider.center();
          float collideeRadius = collidee.radius();
          float colliderRadius = collider.radius();

          float minOffset = colliderRadius + collideeRadius;
          Vector2 offset = colliderCenter - collideeCenter;

          if(offset.LengthSquared() < minOffset*minOffset){
            collidee.owner.CollideFrom(collider.owner);
            collider.owner.CollideTo(collidee.owner);
          }
        }
      }
    }
  }
}

