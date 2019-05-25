/*
 * Quantis Wrapper for C#
 *
 * Copyright (C) 2004-2012 ID Quantique SA, Carouge/Geneva, Switzerland
 * All rights reserved.
 *
 * ----------------------------------------------------------------------------
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions, and the following disclaimer,
 *    without modification.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
 * OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY.
 *
 * ----------------------------------------------------------------------------
 *
 * Alternatively, this software may be distributed under the terms of the
 * terms of the GNU General Public License version 2 as published by the
 * Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License
 * for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307 USA
 *
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.CompilerServices;

namespace idQ
{

  /// <summary>
  /// Define constants of Quantis devices types
  /// </summary>
  enum QuantisDeviceType
  {
    /// <summary>Quantis PCI or PCI-Express</summary>
    QUANTIS_DEVICE_PCI = 1,

    /// <summary>Quantis USB</summary>
    QUANTIS_DEVICE_USB = 2
  };


  /// <summary>
  /// Quantis exception
  /// </summary>
  public class QuantisException : ApplicationException
  {
    /// <summary>
    /// Initializes a new instance of the QuantisException class.
    /// </summary>
    public QuantisException()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the System.ApplicationException class with a 
    /// specified error message.
    /// </summary>
    /// <param name="message">A message that describes the error.</param>
    public QuantisException(string message) : 
      base(message)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the System.ApplicationException class with a 
    /// specified error message and a reference to the inner exception that is the 
    /// cause of this exception.
    /// </summary>
    /// <param name="message">A message that describes the error.</param>
    /// <param name="inner">
    /// The exception that is the cause of the current exception. If the innerException 
    /// parameter is not a null reference, the current exception is raised in a catch 
    /// block that handles the inner exception.
    /// </param>
    public QuantisException(string message, Exception inner) : 
      base(message, inner)
    {
    }
  }


  /// <summary>
  /// Provides methods for configuring Quantis device and read random data.
  /// </summary>
  class Quantis
  {

    private QuantisDeviceType deviceType;
    private uint deviceNumber;

    /// <summary>
    /// Checks if result of a function in the Quantis Library is as error. 
    /// In such a case a QuantisException is thrown
    /// </summary>
    /// <param name="result">The result of the function.</param>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    private static void CheckError(int result)
    {
      // All errors are negative
      if (result < 0)
      {
        throw new QuantisException(ptrToString(QuantisStrError(result)));
      }
    }


    /// <summary>
    /// Convert a unmanaged C DLL 'char*' pointer to a string. 
    /// In C# the char* pointer is marshaled to an IntPtr.
    /// </summary>
    /// <param name="ptr">A IntPtr pointer to covert.</param>
    /// <returns>A string with the content of the unmanaged C DLL char* pointer.</returns>
    private static string ptrToString(IntPtr ptr)
    {
      return ptrToString(ptr, 1024);
    }


    /// <summary>
    /// Convert a unmanaged C DLL 'char*' pointer to a string. 
    /// In C# the char* pointer is marshaled to an IntPtr.
    /// </summary>
    /// <param name="ptr">A IntPtr pointer to covert.</param>
    /// <param name="maxStrLength">The maximal length of the char* string.</param>
    /// <returns>A string with the content of the unmanaged C DLL char* pointer.</returns>
    private static string ptrToString(IntPtr ptr, int maxStrLength)
    {
      if (ptr == IntPtr.Zero)
      {
        return "";
      }

      // Initialize managed container
      byte[] byteArray = new byte[maxStrLength];

      // Copy char* content from the unamanaged memory pointer to the managed container
      System.Runtime.InteropServices.Marshal.Copy(ptr, byteArray, 0, maxStrLength);
      
      // char* are terminated by "\0". Search for the index of this character.
      int idx = 0;
      for (idx = 0; idx < byteArray.Length; ++idx)
      {
        if (byteArray[idx] == 0)
        {
           break;
        }
      }

      // Convert byte array to strings
      return System.Text.Encoding.ASCII.GetString(byteArray, 0, idx);
    }


    /// <summary>
    /// Initializes a new instance of the Quantis class to access the specified device.
    /// </summary>
    /// <param name="deviceType">The type of Quantis device.</param>
    /// <param name="deviceNumber">The Quantis device number. Note that first device is 0.</param>
    public Quantis(QuantisDeviceType deviceType, uint deviceNumber) 
    {
      this.deviceType = deviceType;
      this.deviceNumber = deviceNumber;
    }


    /// <summary>
    /// Resets the Quantis board.
    /// </summary>
    /// <remarks>
    /// This function do not generally has to be called, since the board is automatically reset.
    /// </remarks>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    void BoardReset()
    {
      CheckError(QuantisBoardReset(deviceType, deviceNumber));
    }


    /// <summary>
    /// Returns the number of specific Quantis type devices that have been detected on the system.
    /// </summary>
    /// <param name="deviceType">The type of Quantis device.</param>
    /// <returns>The number of Quantis devices that have been detected on the system.</returns>
    /// <remarks>If an error occurred while counting the devices, 0 is returned.</remarks>
    public int Count()
    {
      return QuantisCount(deviceType);
    }

    /// <summary>
    /// Returns the number of specific Quantis type devices that have been detected on the system.
    /// </summary>
    /// <param name="deviceType">The type of Quantis device.</param>
    /// <returns>The number of Quantis devices that have been detected on the system.</returns>
    /// <remarks>If an error occurred while counting the devices, 0 is returned.</remarks>
    public static int Count(QuantisDeviceType deviceType)
    {
      return QuantisCount(deviceType);
    }


    /// <summary>
    /// Get the version of the board.
    /// </summary>
    /// <returns>The version of the board.</returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public int GetBoardVersion()
    {
      int result = QuantisGetBoardVersion(deviceType, deviceNumber);
      CheckError(result);
      return result;
    }


    /// <summary>
    /// Returns the version of the driver as a decimal number composed by the 
    /// major and minor number: <code>version = major.minor</code>.
    /// </summary>
    /// <returns>The version of the board.</returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public float GetDriverVersion()
    {
      float result = QuantisGetDriverVersion(deviceType);
      CheckError((int)result);
      return result;
    }


    /// <summary>
    /// Returns the library version as a number composed by the major and minor
    /// number: <code>version = major.minor</code>.
    /// </summary>
    /// <returns>The library version.</returns>
    public static float GetLibVersion()
    {
      return QuantisGetLibVersion();
    }


    /// <summary>
    /// Get a String with the manufacturer of the Quantis device.
    /// </summary>
    /// <returns>The manufacturer of the Quantis device or "Not available"
    /// when an error occurred or when the device do not support the operation 
    /// (currently only Quantis USB returns a serial number).
    /// </returns>
    public string GetManufacturer()
    {
      return ptrToString(QuantisGetManufacturer(deviceType, deviceNumber));
    }


    /// <summary>
    /// Returns the number of modules that have been detected on a Quantis device.
    /// </summary>
    /// <returns>the number of detected modules.</returns>
    /// <seealso>GetModulesMask</seealso>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public int GetModulesCount() 
    {
      int result = QuantisGetModulesCount(deviceType, deviceNumber);
      CheckError(result);
      return result;
    }


    /// <summary>
    /// Returns the data rate (in Bytes per second) provided by the Quantis device.
    /// </summary>
    /// <returns>the data rate provided by the Quantis device.</returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public int GetModulesDataRate()
    {
      int result = QuantisGetModulesDataRate(deviceType, deviceNumber);
      CheckError(result);
      return result;
    }


    /// <summary>
    /// Returns a bitmask of the modules that have been detected on a Quantis
    /// device, where bit <em>n</em> is set if module <em>n</em> is present.
    /// For instance when 5 (1101 in binary) is returned, it means that modules
    /// 0, 2 and 3 have been detected.
    /// </summary>
    /// <returns>
    /// A bitmask of the detected modules or a negative value when an error occurred.
    /// </returns>
    /// <seealso>GetModulesStatus</seealso>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public int GetModulesMask()
    {
      int result = QuantisGetModulesMask(deviceType, deviceNumber);
      CheckError(result);
      return result;
    }


    /// <summary>
    /// Get the power status of the modules.
    /// </summary>
    /// <returns>
    /// TRUE if the modules are powered, FALSE if the modules are not powered and
    /// a negative value when an error occured.
    /// </returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public bool GetModulesPower()
    {
      int result = QuantisGetModulesPower(deviceType, deviceNumber);
      CheckError(result);
      return (result != 0);
    }


    /// <summary>
    /// Returns the status of the modules on the device as a bitmask as defined 
    /// in QuantisGetModulesMask. Bit <em>n</em> is set (equal to 1) only when
    /// module <em>n</em> is enabled and functional. 
    /// </summary>
    /// <returns>A bitmask with the status of the modules.</returns>
    /// <seealso>QuantisGetModulesMask</seealso>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public int GetModulesStatus()
    {
      int result = QuantisGetModulesStatus(deviceType, deviceNumber);
      CheckError(result);
      return result;
    }


    /// <summary>
    /// Get a String with the serial number of the Quantis device.
    /// </summary>
    /// <returns>The serial number of the Quantis device or "S/N not available"
    /// when an error occurred or when the device do not support the operation 
    /// (currently only Quantis USB returns a serial number).
    /// </returns>
    public string GetSerialNumber()
    {
      return ptrToString(QuantisGetSerialNumber(deviceType, deviceNumber));
    }


    /// <summary>
    /// Disable one or more modules.
    /// </summary>
    /// <param name="modulesMask">
    /// A bitmask of the modules (as specified in QuantisGetModulesMask) that 
    /// must be disabled.
    /// </param>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public void ModulesDisable(int modulesMask)
    {
      CheckError(QuantisModulesDisable(deviceType, deviceNumber, modulesMask));
    }


    /// <summary>
    /// Enable one or more modules.
    /// </summary>
    /// <param name="modulesMask">
    /// A bitmask of the modules (as specified in QuantisGetModulesMask) that 
    /// must be enabled.
    /// </param>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public void ModulesEnable(int modulesMask)
    {
      CheckError(QuantisModulesEnable(deviceType, deviceNumber, modulesMask));
    }


    /// <summary>
    /// Reset one or more modules.
    /// </summary>
    /// <param name="modulesMask">
    /// A bitmask of the modules (as specified in QuantisGetModulesMask) that 
    /// must be reset.
    /// </param>
    /// <remarks>
    /// This function just call QuantisModulesDisable and then QuantisModulesEnable 
    /// with the provided modulesMask.
    /// </remarks>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public void ModulesReset(int modulesMask)
    {
      CheckError(QuantisModulesReset(deviceType, deviceNumber, modulesMask));
    }


    /// <summary>
    /// Reads random data from the Quantis device.
    /// </summary>
    /// <param name="size">
    /// The number of bytes to read. This value cannot be larger than 
    /// QUANTIS_MAX_READ_SIZE (defined in Quantis.h)
    /// </param>
    /// <returns>A byte array with random data.</returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public byte[] Read(uint size)
    {
      IntPtr bufferSize = new IntPtr((int)size);
      byte[] buffer = new byte[size];
      int result = QuantisRead(deviceType, deviceNumber, buffer, bufferSize);
      CheckError(result);
      if (result != (int)size)
      {
        throw new QuantisException("Read " + result + " bytes instead of " + size);
      }
      return buffer;
    }


    /// <summary>
    /// Reads a random double-precision floating-point number from the 
    /// Quantis device.
    /// </summary>
    /// <returns>A random double-precision floating-point number between 0 and 1.</returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public double ReadDouble()
    {
      double data;
      CheckError(QuantisReadDouble_01(deviceType, deviceNumber, out data));
      return data;
    }


    /// <summary>
    /// Reads a random double-precision floating-point number from the 
    /// Quantis device.
    /// </summary>
    /// <param name="min">The minimal value the random number can take.</param>
    /// <param name="max">The maximal value the random number can take.</param>
    /// <returns>A random double-precision floating-point number.</returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public double ReadDouble(double min, double max)
    {
      double data;
      CheckError(QuantisReadScaledDouble(deviceType, deviceNumber, out data, min, max));
      return data;
    }


    /// <summary>
    /// Reads a random single-precision floating-point number from the 
    /// Quantis device.
    /// </summary>
    /// <returns>A random single-precision floating-point number.</returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public float ReadFloat()
    {
      float data;
      CheckError(QuantisReadFloat_01(deviceType, deviceNumber, out data));
      return data;
    }


    /// <summary>
    /// Reads a random single-precision floating-point number from the 
    /// Quantis device.
    /// </summary>
    /// <param name="min">The minimal value the random number can take.</param>
    /// <param name="max">The maximal value the random number can take.</param>
    /// <returns>A random single-precision floating-point number.</returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public float ReadFloat(float min, float max)
    {
      float data;
      CheckError(QuantisReadScaledFloat(deviceType, deviceNumber, out data, min, max));
      return data;
    }


    /// <summary>
    /// Reads a random 32-bit signed integer from the Quantis device.
    /// </summary>
    /// <returns>A random double-precision floating-point number.</returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public int ReadInt()
    {
      int data;
      CheckError(QuantisReadInt(deviceType, deviceNumber, out data));
      return data;
    }


    /// <summary>
    /// Reads a random 32-bit signed integer from the Quantis device.
    /// </summary>
    /// <param name="min">The minimal value the random number can take.</param>
    /// <param name="max">The maximal value the random number can take.</param>
    /// <returns>A random double-precision floating-point number.</returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public int ReadInt(int min, int max)
    {
      int data;
      CheckError(QuantisReadScaledInt(deviceType, deviceNumber, out data, min, max));
      return data;
    }


    /// <summary>
    /// Reads a random 16-bit signed integer from the Quantis device.
    /// </summary>
    /// <returns>A random double-precision floating-point number.</returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public short ReadShort()
    {
      short data;
      CheckError(QuantisReadShort(deviceType, deviceNumber, out data));
      return data;
    }

    
    /// <summary>
    /// Reads a random 16-bit signed integer from the Quantis device.
    /// </summary>
    /// <param name="min">The minimal value the random number can take.</param>
    /// <param name="max">The maximal value the random number can take.</param>
    /// <returns>A random double-precision floating-point number.</returns>
    /// <exception cref="QuantisException">
    /// Quantis was unable to perform the operation.
    /// </exception>
    public short ReadShort(short min, short max)
    {
      short data;
      CheckError(QuantisReadScaledShort(deviceType, deviceNumber, out data, min, max));
      return data;
    }

    // ************************* Quantis.dll imports *************************
    [DllImport("quantis.dll", EntryPoint = "QuantisBoardReset", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisBoardReset(QuantisDeviceType deviceType,
                                                uint deviceNumber);

    [DllImport("quantis.dll", EntryPoint = "QuantisCount", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisCount(QuantisDeviceType deviceType);

    [DllImport("quantis.dll", EntryPoint = "QuantisGetBoardVersion", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisGetBoardVersion(QuantisDeviceType deviceType, 
                                                     uint deviceNumber);

    [DllImport("quantis.dll", EntryPoint = "QuantisGetDriverVersion", CallingConvention=CallingConvention.Cdecl)]
    private static extern float QuantisGetDriverVersion(QuantisDeviceType deviceType);

    [DllImport("quantis.dll", EntryPoint = "QuantisGetLibVersion", CallingConvention=CallingConvention.Cdecl)]
    private static extern float QuantisGetLibVersion();

    [DllImport("quantis.dll", EntryPoint = "QuantisGetManufacturer", CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr QuantisGetManufacturer(QuantisDeviceType deviceType, 
                                                        uint deviceNumber);

    [DllImport("quantis.dll", EntryPoint = "QuantisGetModulesCount", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisGetModulesCount(QuantisDeviceType deviceType, 
                                                     uint deviceNumber);

    [DllImport("quantis.dll", EntryPoint = "QuantisGetModulesDataRate", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisGetModulesDataRate(QuantisDeviceType deviceType, 
                                                        uint deviceNumber);
    
    [DllImport("quantis.dll", EntryPoint = "QuantisGetModulesMask", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisGetModulesMask(QuantisDeviceType deviceType, 
                                                    uint deviceNumber);

    [DllImport("quantis.dll", EntryPoint = "QuantisGetModulesPower", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisGetModulesPower(QuantisDeviceType deviceType, 
                                                     uint deviceNumber);

    [DllImport("quantis.dll", EntryPoint = "QuantisGetModulesStatus", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisGetModulesStatus(QuantisDeviceType deviceType, 
                                                      uint deviceNumber);

    [DllImport("quantis.dll", EntryPoint = "QuantisGetSerialNumber", CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr QuantisGetSerialNumber(QuantisDeviceType deviceType, 
                                                               uint deviceNumber);

    [DllImport("quantis.dll", EntryPoint = "QuantisModulesDisable", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisModulesDisable(QuantisDeviceType deviceType, 
                                                    uint deviceNumber, 
                                                    int modulesMask);

    [DllImport("quantis.dll", EntryPoint = "QuantisModulesEnable", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisModulesEnable(QuantisDeviceType deviceType, 
                                                   uint deviceNumber,   
                                                   int modulesMask);

    [DllImport("quantis.dll", EntryPoint = "QuantisModulesReset", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisModulesReset(QuantisDeviceType deviceType, 
                                                  uint deviceNumber, 
                                                  int modulesMask);

    [DllImport("quantis.dll", EntryPoint = "QuantisRead", CharSet = CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisRead(QuantisDeviceType deviceType, 
                                          uint deviceNumber,
                                          byte[] data,
                                          IntPtr size);
    
    [DllImport("quantis.dll", EntryPoint = "QuantisReadDouble_01", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisReadDouble_01(QuantisDeviceType deviceType,
                                                   uint deviceNumber,
                                                   out double data);
    
    [DllImport("quantis.dll", EntryPoint = "QuantisReadScaledDouble", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisReadScaledDouble(QuantisDeviceType deviceType, 
                                                      uint deviceNumber,
                                                      out double data,
                                                      double min,
                                                      double max);

    [DllImport("quantis.dll", EntryPoint = "QuantisReadFloat_01", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisReadFloat_01(QuantisDeviceType deviceType,
                                                  uint deviceNumber,
                                                  out float data);

    [DllImport("quantis.dll", EntryPoint = "QuantisReadScaledFloat", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisReadScaledFloat(QuantisDeviceType deviceType,
                                                     uint deviceNumber,
                                                     out float data,
                                                     float min,
                                                     float max);
    
    [DllImport("quantis.dll", EntryPoint = "QuantisReadInt", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisReadInt(QuantisDeviceType deviceType, 
                                             uint deviceNumber,
                                             out int data);
    
    [DllImport("quantis.dll", EntryPoint = "QuantisReadScaledInt", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisReadScaledInt(QuantisDeviceType deviceType,
                                                   uint deviceNumber,
                                                   out int data,
                                                   int min,
                                                   int max);
    
    [DllImport("quantis.dll", EntryPoint = "QuantisReadShort", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisReadShort(QuantisDeviceType deviceType,
                                             uint deviceNumber,
                                             out short data);
    
    [DllImport("quantis.dll", EntryPoint = "QuantisReadScaledShort", CallingConvention=CallingConvention.Cdecl)]
    private static extern int QuantisReadScaledShort(QuantisDeviceType deviceType,
                                                     uint deviceNumber,
                                                     out short data,
                                                     short min,
                                                     short max);

    [DllImport("quantis.dll", EntryPoint = "QuantisStrError", CallingConvention=CallingConvention.Cdecl)]
    private static extern IntPtr QuantisStrError(int errorNumber);

  }
}


