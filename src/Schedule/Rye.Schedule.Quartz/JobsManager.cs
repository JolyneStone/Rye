using System;
using System.Collections.Generic;
using Quartz;
using Quartz.Impl;

namespace Rye.Schedule.Quartz
{
    public class JobsManager
    {
        public List<JobBase> Jobs { get; set; } = new List<JobBase>();

        private readonly ISchedulerFactory schedulerFactory = null;
        private readonly IScheduler scheduler = null;
        public JobsManager()
        {
            schedulerFactory = new StdSchedulerFactory();
            System.Threading.Tasks.Task<IScheduler> _scheduler = schedulerFactory.GetScheduler();
            _scheduler.Wait();
            scheduler = _scheduler.Result;
        }

        public void Start()
        {
            if (schedulerFactory == null || scheduler == null)
            {
                return;
            }
            SetWorkerState(true);

            if (scheduler != null)
            {
                scheduler.Start().Wait();
            }
        }

        public void Stop()
        {
            SetWorkerState(false);
            if (scheduler != null)
            {
                scheduler.Shutdown(true).Wait();
            }
        }

        private void SetWorkerState(bool start)
        {
            if (Jobs != null && Jobs.Count > 0)
            {
                foreach (JobBase job in Jobs)
                {
                    try
                    {
                        if (start)
                        {
                            IJobDetail jobDetail = JobBuilder.Create(job.GetType())
                                                                       .WithIdentity($"job_{job.GetType().ToString()}")
                                                                       .Build();

                            ICronTrigger cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                                                                                .WithIdentity($"trigger_{job.GetType().ToString()}")
                                                                                .WithCronSchedule(job.CronSchedule)
                                                                                .Build();

                            scheduler.ScheduleJob(jobDetail, cronTrigger).Wait();

                            Log.Current.Information("JobsManager", job.GetType().ToString() + " is started.");
                        }
                        else
                        {
                            Log.Current.Information("JobsManager", job.GetType().ToString() + " is stop.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Current.Error("JobsManager", ex.ToString());
                    }
                }
            }
        }
    }
}
