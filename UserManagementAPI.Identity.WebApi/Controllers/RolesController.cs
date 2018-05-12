using System;
using System.Collections.Generic;
using System.Web.Http;
using UserManagementAPI.Common.Models;
using UserManagementAPI.Identity.UserManagementInterfaces;
using UserManagementAPI.Identity.WebAPI.CustomAuthorization;

namespace UserManagementAPI.Identity.WebAPI.Controllers
{
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {
        private IRoleManagement _roleManager;

        public RolesController(IRoleManagement roleManager)
        {
            _roleManager = roleManager;
        }

        [AuthorizationAction(Roles = "Administrator")]
        [Route("{id:guid}", Name = "GetRoleById")]
        public IHttpActionResult GetRole(string Id)
        {
            var role = _roleManager.GetRoleById(Id);

            if (role != null)
            {
                return Ok(role);
            }

            return NotFound();
        }

        [AuthorizationAction(Roles = "Administrator")]
        [Route("", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = _roleManager.GetAllRoles();

            return Ok(roles);
        }

        [AuthorizationAction(Roles = "Administrator")]
        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create([FromBody]CreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createRoleResult = _roleManager.CreateRole(model);

            if (!createRoleResult.IsSuccess)
            {
                return GetErrorResult(createRoleResult);
            }

            Uri locationHeader = new Uri(Url.Link("GetRoleById", new { id = createRoleResult.Result.Id }));

            return Created(locationHeader, createRoleResult.Result);
        }

        [AuthorizationAction(Roles = "Administrator")]
        [Route("{id:guid}")]
        [HttpDelete]
        public IHttpActionResult DeleteRole([FromUri]string Id)
        {
            var deleteRoleResult = _roleManager.DeleteRole(Id);

            if (!deleteRoleResult.IsSuccess)
            {
                return GetErrorResult(deleteRoleResult);
            }

            return Ok();
        }

        [AuthorizationAction(Roles = "Administrator")]
        [Route("user/{id:guid}/roles")]
        [HttpPost]
        public IHttpActionResult AssignRolesToUser([FromUri] string id, [FromBody] IEnumerable<string> rolesToAssign)
        {
            var result = _roleManager.AssignRolesToUser(id, rolesToAssign);

            if (!result.IsSuccess)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }
    }
}