using System;
using Serializables;
using UnityEngine;

namespace Game.Projectile
{
    [CreateAssetMenu(fileName = nameof(ProjectileSettings), menuName = "AAA/Subsystems/ProjectileSettings")]
    internal class ProjectileSettings : ScriptableObject
    {
        [field: SerializeField]
        internal SerializableDictionary<ProjectileType, ProjectileAuthorPair> ProjectileAuthors { get; private set; }
    }

    [Serializable]
    public struct ProjectileAuthorPair
    {
        public ProjectileAuthor Client;
        public ProjectileAuthor Server;
    }
}