using AutoMapper;
using ForumProject.Application.Common.Exceptions;
using ForumProject.Application.Features.Topics.DTOs;
using ForumProject.Application.Features.Topics.Interfaces;
using ForumProject.Domain.Entities;

namespace ForumProject.Application.Features.Topics.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;

        public  TopicService(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;

            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TopicDto, Topic>();
                cfg.CreateMap<Topic, TopicDto>();
            }));
        }

        public async Task<bool> AddAsync(TopicDto topicDto)
        {
            bool isAnyRowChanged = await _topicRepository.AddAsync(_mapper.Map<Topic>(topicDto));

            return isAnyRowChanged;
        }

        public async Task<bool> DeleteAsync(int topicId)
        {
            bool isAnyRowChanged = await _topicRepository.DeleteAsync(topicId);

            return isAnyRowChanged;
        }

        public async Task<IEnumerable<TopicDto>> GetAllAsync()
        {
            IEnumerable<Topic> topics = await _topicRepository.GetAllAsync();
            return topics.Select(_mapper.Map<Topic, TopicDto>);
        }

        public async Task<TopicDto> GetByIdAsync(int topicId)
        {
            Topic? topic = await _topicRepository.GetByIdAsync(topicId);
            if(topic != null)
            {
                return _mapper.Map<TopicDto>(topic);
            }
            throw new NotFoundException(nameof(topic), topicId);
        }

        public async Task<IEnumerable<TopicDto>> GetByPostIdAsync(int postId)
        {
            IEnumerable<Topic> topics = await _topicRepository.GetByPostIdAsync(postId);
            return topics.Select(_mapper.Map<Topic, TopicDto>);
        }

        public async Task<IEnumerable<TopicDto>> GetByUserIdAsync(int userId)
        {
            IEnumerable<Topic> topics = await _topicRepository.GetByUserIdAsync(userId);
            return topics.Select(_mapper.Map<Topic, TopicDto>);
        }
    } 
}