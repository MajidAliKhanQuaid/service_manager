using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ServiceManager.Common.Models
{
    public enum ServiceStatus
    {
        [Display(Name = "Pending Refresh")]
        PendingRefresh = 0,
        //
        // Summary:
        //     The service is not running. This corresponds to the Win32 SERVICE_STOPPED constant,
        //     which is defined as 0x00000001.
        [Display(Name = "Stopped")]
        Stopped = 1,
        //
        // Summary:
        //     The service is starting. This corresponds to the Win32 SERVICE_START_PENDING
        //     constant, which is defined as 0x00000002.
        [Display(Name = "Start Pending")]
        StartPending = 2,
        //
        // Summary:
        //     The service is stopping. This corresponds to the Win32 SERVICE_STOP_PENDING constant,
        //     which is defined as 0x00000003.
        [Display(Name = "Stop Pending")]
        StopPending = 3,
        //
        // Summary:
        //     The service is running. This corresponds to the Win32 SERVICE_RUNNING constant,
        //     which is defined as 0x00000004.
        [Display(Name = "Running")]
        Running = 4,
        //
        // Summary:
        //     The service continue is pending. This corresponds to the Win32 SERVICE_CONTINUE_PENDING
        //     constant, which is defined as 0x00000005.
        [Display(Name = "Continue Pending")]
        ContinuePending = 5,
        //
        // Summary:
        //     The service pause is pending. This corresponds to the Win32 SERVICE_PAUSE_PENDING
        //     constant, which is defined as 0x00000006.
        [Display(Name = "Pause Pending")]
        PausePending = 6,
        //
        // Summary:
        //     The service is paused. This corresponds to the Win32 SERVICE_PAUSED constant,
        //     which is defined as 0x00000007.
        [Display(Name = "Paused")]
        Paused = 7,
        [Display(Name = "Not Found")]
        NotFound = 8,
    }
}
