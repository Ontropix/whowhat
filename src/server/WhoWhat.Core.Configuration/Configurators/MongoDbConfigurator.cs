using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using Platform.Domain;

namespace WhoWhat.Core.Configuration
{
    internal class MongoDbConfigurator
    {
        internal void RegisterMongoDbConventions()
        {
            var appConventions = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new NamedIdMemberConvention("Id"),
            };

            BsonClassMap.RegisterClassMap<EventMetadata>();
            ConventionRegistry.Register("Core", appConventions, type => true);

            BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(new DateTimeSerializationOptions(DateTimeKind.Utc)));
            BsonSerializer.RegisterSerializer(typeof(DateTime?), new NullableSerializer<DateTime>());
        }
    }
}