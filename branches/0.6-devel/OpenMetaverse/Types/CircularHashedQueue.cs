﻿/*
 * Copyright (c) 2009, openmetaverse.org
 * All rights reserved.
 *
 * - Redistribution and use in source and binary forms, with or without
 *   modification, are permitted provided that the following conditions are met:
 *
 * - Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 * - Neither the name of the openmetaverse.org nor the names
 *   of its contributors may be used to endorse or promote products derived from
 *   this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;

namespace OpenMetaverse
{
    /// <summary>
    /// A circular queue with a hash-based Contains function
    /// </summary>
    public class CircularHashedQueue<T>
    {
        public readonly T[] Items;

        Dictionary<T, bool> hashSet;
        int first;
        int next;
        int capacity;
        object syncRoot;

        public int First { get { return first; } }
        public int Next { get { return next; } }

        public CircularHashedQueue(int capacity)
        {
            this.capacity = capacity;
            Items = new T[capacity];
            hashSet = new Dictionary<T, bool>(capacity);
            syncRoot = new object();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="queue">Circular hashed queue to copy</param>
        public CircularHashedQueue(CircularHashedQueue<T> queue)
        {
            lock (queue.syncRoot)
            {
                capacity = queue.capacity;
                Items = new T[capacity];
                hashSet = new Dictionary<T, bool>(queue.hashSet);
                syncRoot = new object();

                for (int i = 0; i < capacity; i++)
                    Items[i] = queue.Items[i];

                first = queue.first;
                next = queue.next;
            }
        }

        public void Clear()
        {
            lock (syncRoot)
            {
                // Explicitly remove references to help garbage collection
                for (int i = 0; i < capacity; i++)
                    Items[i] = default(T);

                first = next;

                hashSet.Clear();
            }
        }

        public bool Contains(T value)
        {
            return hashSet.ContainsKey(value);
        }

        public void Enqueue(T value)
        {
            lock (syncRoot)
            {
                Items[next] = value;
                next = (next + 1) % capacity;
                if (next == first) first = (first + 1) % capacity;

                hashSet[value] = true;
            }
        }

        public bool TryEnqueue(T value)
        {
            lock (syncRoot)
            {
                if (hashSet.ContainsKey(value))
                    return false;

                Items[next] = value;
                next = (next + 1) % capacity;
                if (next == first) first = (first + 1) % capacity;

                hashSet[value] = true;

                return true;
            }
        }

        public T Dequeue()
        {
            lock (syncRoot)
            {
                T value = Items[first];
                Items[first] = default(T);

                if (first != next)
                    first = (first + 1) % capacity;

                hashSet.Remove(value);

                return value;
            }
        }

        public T DequeueLast()
        {
            lock (syncRoot)
            {
                // If the next element is right behind the first element (queue is full),
                // back up the first element by one
                int firstTest = first - 1;
                if (firstTest < 0) firstTest = capacity - 1;

                if (firstTest == next)
                {
                    --next;
                    if (next < 0) next = capacity - 1;

                    --first;
                    if (first < 0) first = capacity - 1;
                }
                else if (first != next)
                {
                    --next;
                    if (next < 0) next = capacity - 1;
                }

                T value = Items[next];
                Items[next] = default(T);

                hashSet.Remove(value);

                return value;
            }
        }
    }
}
