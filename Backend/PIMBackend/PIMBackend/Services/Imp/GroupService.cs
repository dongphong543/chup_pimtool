﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PIMBackend.Domain.Entities;
using PIMBackend.Domain.Objects;
using PIMBackend.Errors;
using PIMBackend.Repositories;

namespace PIMBackend.Services.Imp
{
    /// <summary>
    ///     Implementation of group service
    /// </summary>
    public class GroupService : BaseService, IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public IEnumerable<Group> Get()
        {
            return _groupRepository.Get().Include(g => g.GroupLeader);
        }

        public IEnumerable<Group> Get(Filter filter)
        {
            return _groupRepository.Get();
        }

        public Group Get(decimal id)
        {
            var ret = _groupRepository.Get().Include(g => g.GroupLeader).SingleOrDefault(x => x.Id == id);
            if (ret == null)
            {
                throw new IdNotExistException();
            }

            return ret;
        }

        //return type
        public void Create(Group group)
        {
            if (Get(group.Id) != null)
            {
                throw new IdAlreadyExistException();
            }

            else
            {
                group.Version = 0;
                _groupRepository.Add(group);

                try
                {
                    _groupRepository.SaveChange();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new UpdateConflictException();
                }
            }

        }

        public Group Update(Group group)
        {
            var groupDb = _groupRepository.Get(group.Id);
            if (groupDb == null)
            {
                throw new ArgumentException();
            }


            if (groupDb.Version != group.Version)
            {
                throw new UpdateConflictException();
            }

            else
            {
                groupDb.GroupLeaderId = group.GroupLeaderId;
                groupDb.Version += 1;
            }

            try
            {
                _groupRepository.SaveChange();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new UpdateConflictException();
            }

            return groupDb;
        }

        public void Delete(decimal id)
        {
            _groupRepository.Delete(id);
            _groupRepository.SaveChange();
        }
    }
}