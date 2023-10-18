using GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GraphQLParser.AST;
using GraphQL.Types;
using Graphd.Graph.Extensions;

namespace Graphd.Graph.Resolvers.Queries;

internal class Recursion
{

    protected IResolveFieldContext context;


    public Recursion(IResolveFieldContext context)
    {
        this.context = context;
    }

    public List<RecursionField> ToList()
    {

        var fields = new List<RecursionField>();
        var type = context.FieldDefinition.Type.GetGenericArguments().First().GetGenericArguments()[0];
        context
            ?.SubFields
            ?.ToList()
            .ForEach(pair =>
            {
                ForEach(type, pair.Value.Field, null, fields);
            });
        return fields;
    }

    protected void ForEach(Type type, ASTNode aField, RecursionField? parent, List<RecursionField> fields)
    {
        var field = (aField as GraphQLField)!;
        var fieldName = field.Name.ToString();
        var subFieldType = GetProperty(type, fieldName.Capitalize());

        var recursionField = new RecursionField(subFieldType.Name, subFieldType.PropertyType, field.Arguments, parent);
        fields.Add(recursionField);

        if (!FieldHasChildren(field))
        {
            return;
        }

        GetChildren(field)
            .ForEach(child => ForEach(
                GetTypeOfField(subFieldType),
                child,
                recursionField,
                fields
            ));
    }

    protected Type GetTypeOfField(PropertyInfo property)
    {
        var typeOfField = property.PropertyType;

        if (typeOfField.IsGenericType)
            typeOfField = typeOfField.GenericTypeArguments.FirstOrDefault();

        return typeOfField;
    }

    protected List<ASTNode> GetChildren(GraphQLField field)
    {
        return field.SelectionSet!.Selections;
    }

    protected bool FieldHasChildren(GraphQLField field)
    {
        return null != field.SelectionSet;
    }

    //protected void IncludeField(RecursionField field)
    //{
    //    query = query.Include(field.ToString());
    //}

    protected PropertyInfo GetProperty(Type type, string name)
    {
        var property = type.GetProperty(name);
        if (null == property)
        {
            property = type.GetProperty(name);
        }
        return property;
    }
}
