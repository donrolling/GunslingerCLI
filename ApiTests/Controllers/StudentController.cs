﻿using AutoFixture;
using Microservice_API1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Microservice_API1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class StudentController : ControllerBase
	{
		private readonly Fixture _fixture;

		public StudentController()
		{
			_fixture = new Fixture();
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<Student> Get(int id)
		{
			var student = _fixture.Build<Student>().Without(a => a.Id).Create();
			student.Id = id;
			return student;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<Student> Create(Student student)
		{
			return Ok(student);
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<Student> Update(Student student)
		{
			return Ok(student);
		}

		[HttpDelete]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult Delete(Student student)
		{
			return Ok();
		}
	}
}