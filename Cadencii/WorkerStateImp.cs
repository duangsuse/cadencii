/*
 * WorkerStateImp.cs
 * Copyright © 2011 kbinani
 *
 * This file is part of org.kbinani.cadencii.
 *
 * org.kbinani.cadencii is free software; you can redistribute it and/or
 * modify it under the terms of the GPLv3 License.
 *
 * org.kbinani.cadencii is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 */
#if JAVA

package org.kbinani.cadencii;

#elif __cplusplus

namespace org{
namespace kbinani{
namespace cadencii{

#else

namespace org.kbinani.cadencii
{
    using boolean = System.Boolean;
#endif

#if JAVA
    public class WorkerStateImp implements WorkerState
#else
    public class WorkerStateImp : WorkerState
#endif
    {
        private boolean mCancelRequested;

        public double getJobAmount()
        {
            return 0;
        }

        public double getProcessedAmount()
        {
            return 0;
        }

        public boolean isCancelRequested()
        {
            return mCancelRequested;
        }

        public void reportComplete()
        {
        }

        public void reportProgress( double prog )
        {
        }

        public void requestCancel()
        {
            mCancelRequested = true;
        }

        public void reset()
        {
            mCancelRequested = false;
        }
    }

#if JAVA
#elif __cplusplus
} } }
#else
}
#endif