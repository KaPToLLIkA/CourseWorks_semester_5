#include "test_functions.h"


double getCPUTime()
{
#if defined(_WIN32)
    /* Windows -------------------------------------------------- */
    FILETIME createTime;
    FILETIME exitTime;
    FILETIME kernelTime;
    FILETIME userTime;
    if (GetProcessTimes(GetCurrentProcess(),
        &createTime, &exitTime, &kernelTime, &userTime) != -1)
    {
        SYSTEMTIME userSystemTime;
        if (FileTimeToSystemTime(&userTime, &userSystemTime) != -1)
            return (double)userSystemTime.wHour * 3600.0 +
            (double)userSystemTime.wMinute * 60.0 +
            (double)userSystemTime.wSecond +
            (double)userSystemTime.wMilliseconds / 1000.0;
    }

#elif defined(__unix__) || defined(__unix) || defined(unix) || (defined(__APPLE__) && defined(__MACH__))
    /* AIX, BSD, Cygwin, HP-UX, Linux, OSX, and Solaris --------- */

#if defined(_POSIX_TIMERS) && (_POSIX_TIMERS > 0)
    /* Prefer high-res POSIX timers, when available. */
    {
        clockid_t id;
        struct timespec ts;
#if _POSIX_CPUTIME > 0
        /* Clock ids vary by OS.  Query the id, if possible. */
        if (clock_getcpuclockid(0, &id) == -1)
#endif
#if defined(CLOCK_PROCESS_CPUTIME_ID)
            /* Use known clock id for AIX, Linux, or Solaris. */
            id = CLOCK_PROCESS_CPUTIME_ID;
#elif defined(CLOCK_VIRTUAL)
            /* Use known clock id for BSD or HP-UX. */
            id = CLOCK_VIRTUAL;
#else
            id = (clockid_t)-1;
#endif
        if (id != (clockid_t)-1 && clock_gettime(id, &ts) != -1)
            return (double)ts.tv_sec +
            (double)ts.tv_nsec / 1000000000.0;
    }
#endif

#if defined(RUSAGE_SELF)
    {
        struct rusage rusage;
        if (getrusage(RUSAGE_SELF, &rusage) != -1)
            return (double)rusage.ru_utime.tv_sec +
            (double)rusage.ru_utime.tv_usec / 1000000.0;
    }
#endif

#if defined(_SC_CLK_TCK)
    {
        const double ticks = (double)sysconf(_SC_CLK_TCK);
        struct tms tms;
        if (times(&tms) != (clock_t)-1)
            return (double)tms.tms_utime / ticks;
    }
#endif

#if defined(CLOCKS_PER_SEC)
    {
        clock_t cl = clock();
        if (cl != (clock_t)-1)
            return (double)cl / (double)CLOCKS_PER_SEC;
    }
#endif

#endif

    return -1;      /* Failed. */
}

#define INTERPOLATION_K 5
std::vector<entry> new_vector_for_interpol_s(size_t size)
{
    std::vector<entry> new_v;
    for (size_t i = 1; i <= size; ++i) 
    {
        new_v.push_back(entry(i * INTERPOLATION_K));
    }
    return new_v;
}

std::vector<entry> new_vector_for_binary_s(size_t size)
{
    std::vector<entry> new_v;
    for (size_t i = 1; i <= size; ++i)
    {
        new_v.push_back(entry(i));
    }
    return new_v;
}

std::vector<entry> new_vector_for_fibonacci_s(size_t size)
{
    return new_vector_for_binary_s(size);
}


#ifdef _DEBUG
void basic_binary_search_test()
{
    std::cout << "BINARY SEARCH:\n";
    {
        std::vector<entry> data = new_vector_for_binary_s(100);
        for (int i = 1; i <= 100; ++i)
        {
            entry target(i);

            std::cout << "T1:" << binary_search(data, target) << std::endl;
        }
    }

    {
        std::vector<entry> data = new_vector_for_binary_s(100);
        entry target(10000);

        std::cout << "T2:" << binary_search(data, target) << std::endl;
    }

    {
        std::vector<entry> data;
        entry target(10000);

        std::cout << "T3:" << binary_search(data, target) << std::endl;
    }

    {
        std::vector<entry> data = new_vector_for_binary_s(1);
        entry target(1);

        std::cout << "T4:" << binary_search(data, target) << std::endl;
    }

    {
        std::vector<entry> data;
        for (uint64_t i = 100; i < 200; ++i) {
            data.push_back(entry(i));
        }

        entry target(1);

        std::cout << "T5:" << binary_search(data, target) << std::endl;
    }
}

void basic_fibonacci_search_test()
{
    std::cout << "FIBONACCI SEARCH:\n";
    {
        std::vector<entry> data = new_vector_for_fibonacci_s(100);
        for (int i = 1; i <= 100; ++i)
        {
            entry target(i);

            std::cout << "T1:" << fibonacci_search(data, target) << std::endl;
        }
    }

    {
        std::vector<entry> data = new_vector_for_fibonacci_s(100);
        entry target(10000);

        std::cout << "T2:" << fibonacci_search(data, target) << std::endl;
    }

    {
        std::vector<entry> data;
        entry target(10000);

        std::cout << "T3:" << fibonacci_search(data, target) << std::endl;
    }

    {
        std::vector<entry> data = new_vector_for_fibonacci_s(1);
        entry target(1);

        std::cout << "T4:" << fibonacci_search(data, target) << std::endl;
    }

    {
        std::vector<entry> data;
        for (uint64_t i = 100; i < 200; ++i) {
            data.push_back(entry(i));
        }

        entry target(1);

        std::cout << "T5:" << fibonacci_search(data, target) << std::endl;
    }
}
void basic_interpolation_search_test()
{
    std::cout << "INTERPOLATION SEARCH:\n";
    {
        std::vector<entry> data = new_vector_for_interpol_s(100);
        for (int i = 1; i <= 100; ++i)
        {
            entry target(i * INTERPOLATION_K);

            std::cout << "T1:" << interpolation_search(data, target) << std::endl;
        }
    }

    {
        std::vector<entry> data = new_vector_for_interpol_s(100);
        entry target(10000);

        std::cout << "T2:" << interpolation_search(data, target) << std::endl;
    }

    {
        std::vector<entry> data;
        entry target(10000);

        std::cout << "T3:" << interpolation_search(data, target) << std::endl;
    }

    {
        std::vector<entry> data = new_vector_for_interpol_s(1);
        entry target(INTERPOLATION_K);

        std::cout << "T4:" << interpolation_search(data, target) << std::endl;
    }

    {
        std::vector<entry> data;
        for (uint64_t i = 100; i < 200; ++i) {
            data.push_back(entry(i * INTERPOLATION_K));
        }

        entry target(INTERPOLATION_K);

        std::cout << "T5:" << interpolation_search(data, target) << std::endl;
    }

}

#undef INTERPOLATION_K
#endif // _DEBUG


