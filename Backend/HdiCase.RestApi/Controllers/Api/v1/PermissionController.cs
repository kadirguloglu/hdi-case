using System.Reflection;
using HdiCase.RestApi.Controllers.Api.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class PermissionController : BaseController
{
    List<PermissionObject> permissions = new List<PermissionObject>();
    [AllowAnonymous]
    [HttpGet("GetPermissions")]
    public List<PermissionCategoryList> GetPermissions()
    {
        var permissionCategoryEnum = Enum.GetValues(typeof(Enum_PermissionCategory)).Cast<Enum_PermissionCategory>().ToList();
        List<PermissionCategoryObject> permissionCategories = new List<PermissionCategoryObject>();
        foreach (var permissionCategory in permissionCategoryEnum)
        {
            permissionCategories.Add(new PermissionCategoryObject()
            {
                Key = (int)permissionCategory,
                Value = permissionCategory.ToString()
            });
        }

        var permissionEnum = Enum.GetValues(typeof(Enum_Permission)).Cast<Enum_Permission>().ToList();
        foreach (var permission in permissionEnum)
        {
            Type type = permission.GetType();
            FieldInfo? field = type.GetField(permission.ToString());
            if (field is null)
            {
                continue;
            }
            int? permissionCategoryKey = null;
            Attribute? attr = Attribute.GetCustomAttribute(field, typeof(PermissionCategoryAttribute), false);
            if (attr != null)
            {
                permissionCategoryKey = (int)((PermissionCategoryAttribute)attr)._category;
            }
            int? parentPermissionKey = null;
            Attribute? attr1 = Attribute.GetCustomAttribute(field, typeof(ParentPermissionAttribute), false);
            if (attr1 != null)
            {
                parentPermissionKey = (int)((ParentPermissionAttribute)attr1)._category;
            }
            permissions.Add(new PermissionObject()
            {
                Key = (int)permission,
                Value = permission.ToString(),
                CategoryKey = permissionCategoryKey,
                ParentPermissionKey = parentPermissionKey
            });
        }

        List<PermissionCategoryList> permissionCategoryLists = new List<PermissionCategoryList>();
        foreach (var permissionCategory in permissionCategories)
        {
            PermissionCategoryList category = new PermissionCategoryList()
            {
                CategoryKey = permissionCategory.Key,
                CategoryName = permissionCategory.Value,
                Permissions = new List<PermissionList>()
            };
            var parentPermissions = permissions.Where(x => x.CategoryKey == permissionCategory.Key);
            foreach (var permission in parentPermissions)
            {
                var parentPermission = new PermissionList()
                {
                    Value = permission.Value,
                    Key = permission.Key,
                    ChildPermission = new List<PermissionList>()
                };
                FillChildPermissions(parentPermission);
                category.Permissions.Add(parentPermission);
            }
            permissionCategoryLists.Add(category);
        }

        void FillChildPermissions(PermissionList parentPermission)
        {
            var parentPermissions = permissions.Where(x => x.ParentPermissionKey == parentPermission.Key);
            foreach (var permission in parentPermissions)
            {
                var currentPermissions = new PermissionList()
                {
                    Value = permission.Value,
                    Key = permission.Key,
                    ChildPermission = new List<PermissionList>()
                };
                FillChildPermissions(currentPermissions);
                if (parentPermission.ChildPermission != null)
                {
                    parentPermission.ChildPermission.Add(currentPermissions);
                }
            }
        }

        return permissionCategoryLists;
    }

    public class PermissionCategoryObject
    {
        public int Key { get; set; }
        public required string Value { get; set; }
    }

    public class PermissionObject
    {
        public int Key { get; set; }
        public required string Value { get; set; }
        public int? CategoryKey { get; set; }
        public int? ParentPermissionKey { get; set; }
    }

    public class PermissionCategoryList
    {
        public int CategoryKey { get; set; }
        public required string CategoryName { get; set; }
        public List<PermissionList>? Permissions { get; set; }
    }

    public class PermissionList
    {
        public int Key { get; set; }
        public required string Value { get; set; }
        public List<PermissionList>? ChildPermission { get; set; }
    }
}