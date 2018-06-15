﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Int256.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
//   Copyright (c) 2018 Nivloc Enterprises Ltd. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using BigMath.Utils;

namespace BigMath
	{
	/// <summary>
	///     Represents a 256-bit unsigned integer.
	/// </summary>
#if !NETCF
	[DebuggerDisplay ("{DebuggerDisplay,nq}")]
#endif
	[StructLayout (LayoutKind.Explicit,
#if !NETCF
	Pack = 1,
#endif
		Size = 32)]
	public struct UInt256 : IComparable<UInt256>, IComparable, IEquatable<UInt256>, IFormattable, IConvertible
		{
#if !NETCF
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
#endif
		[FieldOffset (0)]
		private ulong _d;

#if !NETCF
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
#endif
		[FieldOffset (8)]
		private ulong _c;

#if !NETCF
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
#endif
		[FieldOffset (16)]
		private ulong _b;

#if !NETCF
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
#endif
		[FieldOffset (24)]
		private ulong _a;

#if !NETCF
		[DebuggerBrowsable (DebuggerBrowsableState.Never)]
		private string DebuggerDisplay
			{
			get { return "0x" + ToString ("X1"); }
			}
#endif

		/// <summary>
		///     Gets a value that represents the number 0 (zero).
		/// </summary>
		public readonly static UInt256 Zero = GetZero ();

		/// <summary>
		///     Represents the largest possible value of an UInt256.
		/// </summary>
		public readonly static UInt256 MaxValue = GetMaxValue ();

		/// <summary>
		///     Represents the smallest possible value of an UInt256.
		/// </summary>
		public readonly static UInt256 MinValue = Zero;

		private static readonly UInt256 MaxTest = MaxValue / 10;

		private static UInt256 GetMaxValue ()
			{
			return new UInt256 (ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue);
			}

		private static UInt256 GetZero ()
			{
			return new UInt256 ();
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (byte value)
			{
			_a = 0;
			_b = 0;
			_c = 0;
			_d = value;
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">if set to <c>true</c> [value].</param>
		public UInt256 (bool value)
			{
			_a = 0;
			_b = 0;
			_c = 0;
			_d = (ulong)(value ? 1 : 0);
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (char value)
			{
			_a = 0;
			_b = 0;
			_c = 0;
			_d = value;
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (decimal value)
			{
			bool isNegative = value < 0;
			uint[] bits = decimal.GetBits (value).ConvertAll (i => (uint)i);
			uint scale = (bits[3] >> 16) & 0x1F;
			if (scale > 0)
				{
				uint[] quotient;
				uint[] reminder;
				MathUtils.DivModUnsigned (bits, new[] {10U * scale}, out quotient, out reminder);

				bits = quotient;
				}

			_a = 0;
			_b = 0;
			_c = bits[2];
			_d = bits[0] | (ulong)bits[1] << 32;

			if (isNegative)
				Negate ();
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (double value)
			: this ((decimal)value)
			{
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (float value)
			: this ((decimal)value)
			{
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (short value)
			: this ((int)value)
			{
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (int value)
			: this ((long)value)
			{
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (long value)
			{
			_a = _b = _c = unchecked((ulong)(value < 0 ? ~0 : 0));
			_d = (ulong)value;
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (sbyte value)
			: this ((long)value)
			{
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (ushort value)
			{
			_a = 0;
			_b = 0;
			_c = 0;
			_d = value;
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (uint value)
			{
			_a = 0;
			_b = 0;
			_c = 0;
			_d = value;
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (ulong value)
			{
			_a = 0;
			_b = 0;
			_c = 0;
			_d = value;
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (Guid value)
			{
			var uint256 = value.ToByteArray ().ToUInt256 (0);
			_a = uint256.A;
			_b = uint256.B;
			_c = uint256.C;
			_d = uint256.D;
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public UInt256 (Int128 value)
			{
			ulong[] values = value.ToUIn64Array ();
			_a = _b = unchecked((ulong)(value.Sign < 0 ? ~0 : 0));
			_c = values[1];
			_d = values[0];
			}

		public UInt256 (UInt128 value)
			{
			ulong[] values = value.ToUIn64Array ();
			_a = _b = 0;
			_c = values[1];
			_d = values[0];
			}

		public UInt256 (Int256 value)
			{
			ulong[] values = value.ToUIn64Array ();
			_a = values[3];
			_b = values[2];
			_c = values[1];
			_d = values[0];
			}

		public UInt256 (UInt256 value)
			{
			ulong[] values = value.ToUIn64Array ();
			_a = values[3];
			_b = values[2];
			_c = values[1];
			_d = values[0];
			}

		public UInt256 (ulong a, ulong b, ulong c, ulong d)
			{
			_a = a;
			_b = b;
			_c = c;
			_d = d;
			}

		/// <summary>
		///     Initializes a new instance of the <see cref="UInt256" /> struct.
		/// </summary>
		/// <param name="sign">The sign.</param>
		/// <param name="ints">The ints.</param>
		public UInt256 (int sign, uint[] ints)
			{
			if (sign < 0)
				throw new ArgumentException ("sign");

			if (ints == null)
				throw new ArgumentNullException ("ints");

			if (sign == 0)
				{
				if (!Array.TrueForAll (ints, i => i == 0))
					throw new ArgumentOutOfRangeException ("ints", "All ints must be zero");
				_a =_b = _c = _d = 0;
				}

			var value = new ulong[4];
			for (int i = 0; i < ints.Length && i < 8; i++)
				Buffer.BlockCopy (ints[i].ToBytes (), 0, value, i * 4, 4);

			_a = value[3];
			_b = value[2];
			_c = value[1];
			_d = value[0];
			}

		/// <summary>
		///     Higher 64 bits of the higher 128 bits.
		/// </summary>
		public ulong A
			{
			get { return _a; }
			}

		/// <summary>
		///     Lower 64 bits of the higher 128 bits.
		/// </summary>
		public ulong B
			{
			get { return _b; }
			}

		/// <summary>
		///     Higher 64 bits of the lower 128 bits.
		/// </summary>
		public ulong C
			{
			get { return _c; }
			}

		/// <summary>
		///     Lower 64 bits of the lower 128 bits.
		/// </summary>
		public ulong D
			{
			get { return _d; }
			}

		/// <summary>
		///     Gets a number that indicates the sign (positive, or zero) of the current UInt256 object.
		/// </summary>
		/// <value>A number that indicates the sign of the UInt256 object</value>
		public int Sign
			{
			get
				{
				if (_a == 0 && _b == 0 && _c == 0 && _d == 0)
					return 0;

				return 1;
				}
			}

		/// <summary>
		///     Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </returns>
		public override int GetHashCode ()
			{
			return _a.GetHashCode () ^ _b.GetHashCode () ^ _c.GetHashCode () ^ _d.GetHashCode ();
			}

		/// <summary>
		///     Returns a value indicating whether this instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		///     true if obj has the same value as this instance; otherwise, false.
		/// </returns>
		public override bool Equals (object obj)
			{
			return base.Equals (obj);
			}

		/// <summary>
		///     Returns a value indicating whether this instance is equal to a specified Int64 value.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns>
		///     true if obj has the same value as this instance; otherwise, false.
		/// </returns>
		public bool Equals (UInt256 obj)
			{
			return _a == obj._a && _b == obj._b && _c == obj._c && _d == obj._d;
			}

		/// <summary>
		///     Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		///     A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString ()
			{
			return ToString (null, null);
			}

#if NETCF
		/// <summary>
		///     Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="format">The format. Only x, X, g, G, d, D are supported.</param>
		/// <returns>
		///     A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public string ToString (string format)
			{
			return ToString (format, null);
			}
		/// <summary>
		///     Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="format">The format. Only x, X, g, G, d, D are supported.</param>
		/// <param name="formatProvider">An object that supplies culture-specific formatting information about this instance.</param>
		/// <returns>
		///     A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public string ToString (string format, IFormatProvider formatProvider)
#else
		/// <summary>
		///     Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <param name="format">The format. Only x, X, g, G, d, D are supported.</param>
		/// <param name="formatProvider">An object that supplies culture-specific formatting information about this instance.</param>
		/// <returns>
		///     A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public string ToString (string format, IFormatProvider formatProvider = null)
#endif
			{
			if (formatProvider == null)
				formatProvider = CultureInfo.CurrentCulture;

			if (!string.IsNullOrEmpty (format))
				{
				char ch = format[0];
				if ((ch == 'x') || (ch == 'X'))
					{
					int min;
					Int32Ex.TryParse (format.Substring (1).Trim (), out min);
					return this.ToBytes (false).ToHexString (ch == 'X', min, false, /*trimZeros:*/ true);
					}

				if (((ch != 'G') && (ch != 'g')) && ((ch != 'D') && (ch != 'd')))
					throw new NotSupportedException ("Not supported format: " + format);
				}

			return ToString ((NumberFormatInfo)formatProvider.GetFormat (typeof(NumberFormatInfo)));
			}

		private string ToString (NumberFormatInfo info)
			{
			if (Sign == 0)
				return "0";

			var sb = new StringBuilder ();
			var ten = new UInt256 (10);
			UInt256 current = this;
			while (true)
				{
				UInt256 r;
				current = DivRem (current, ten, out r);
				if (r._d > 0 || current.Sign != 0 || (sb.Length == 0))
					sb.Insert (0, (char)('0' + r._d));
				if (current.Sign == 0)
					break;
				}

			string s = sb.ToString ();

			return s;
			}

		/// <summary>
		///     Converts the numeric value to an equivalent object. The return value indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="conversionType">The target conversion type.</param>
		/// <param name="provider">An object that supplies culture-specific information about the conversion.</param>
		/// <param name="asLittleEndian">As little endian.</param>
		/// <param name="value">
		///     When this method returns, contains the value that is equivalent to the numeric value, if the
		///     conversion succeeded, or is null if the conversion failed. This parameter is passed uninitialized.
		/// </param>
		/// <returns>true if this value was converted successfully; otherwise, false.</returns>
		public bool TryConvert (Type conversionType, IFormatProvider provider, bool asLittleEndian, out object value)
			{
			if (conversionType == typeof(bool))
				{
				value = (bool)this;
				return true;
				}

			if (conversionType == typeof(byte))
				{
				value = (byte)this;
				return true;
				}

			if (conversionType == typeof(char))
				{
				value = (char)this;
				return true;
				}

			if (conversionType == typeof(decimal))
				{
				value = (decimal)this;
				return true;
				}

			if (conversionType == typeof(double))
				{
				value = (double)this;
				return true;
				}

			if (conversionType == typeof(short))
				{
				value = (short)this;
				return true;
				}

			if (conversionType == typeof(int))
				{
				value = (int)this;
				return true;
				}

			if (conversionType == typeof(long))
				{
				value = (long)this;
				return true;
				}

			if (conversionType == typeof(sbyte))
				{
				value = (sbyte)this;
				return true;
				}

			if (conversionType == typeof(float))
				{
				value = (float)this;
				return true;
				}

			if (conversionType == typeof(string))
				{
				value = ToString (null, provider);
				return true;
				}

			if (conversionType == typeof(ushort))
				{
				value = (ushort)this;
				return true;
				}

			if (conversionType == typeof(uint))
				{
				value = (uint)this;
				return true;
				}

			if (conversionType == typeof(ulong))
				{
				value = (ulong)this;
				return true;
				}

			if (conversionType == typeof(byte[]))
				{
				value = this.ToBytes (asLittleEndian);
				return true;
				}

			if (conversionType == typeof(Guid))
				{
				value = new Guid (this.ToBytes (asLittleEndian));
				return true;
				}

			if (conversionType == typeof(Int128))
				{
				value = (Int128)this;
				return true;
				}

			if (conversionType == typeof(UInt128))
				{
				value = (UInt128)this;
				return true;
				}

			if (conversionType == typeof(Int256))
				{
				value = (Int256)this;
				return true;
				}

			value = null;
			return false;
			}

		/// <summary>
		///     Converts the string representation of a number to its UInt256 equivalent.
		/// </summary>
		/// <param name="value">A string that contains a number to convert.</param>
		/// <returns>
		///     A value that is equivalent to the number specified in the value parameter.
		/// </returns>
		public static UInt256 Parse (string value)
			{
			return Parse (value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
			}

		/// <summary>
		///     Converts the string representation of a number in a specified style format to its UInt256 equivalent.
		/// </summary>
		/// <param name="value">A string that contains a number to convert.</param>
		/// <param name="style">A bitwise combination of the enumeration values that specify the permitted format of value.</param>
		/// <returns>
		///     A value that is equivalent to the number specified in the value parameter.
		/// </returns>
		public static UInt256 Parse (string value, NumberStyles style)
			{
			return Parse (value, style, NumberFormatInfo.CurrentInfo);
			}

		/// <summary>
		///     Converts the string representation of a number in a culture-specific format to its UInt256 equivalent.
		/// </summary>
		/// <param name="value">A string that contains a number to convert.</param>
		/// <param name="provider">An object that provides culture-specific formatting information about value.</param>
		/// <returns>
		///     A value that is equivalent to the number specified in the value parameter.
		/// </returns>
		public static UInt256 Parse (string value, IFormatProvider provider)
			{
			return Parse (value, NumberStyles.Integer, NumberFormatInfo.GetInstance (provider));
			}

		/// <summary>
		///     Converts the string representation of a number in a specified style and culture-specific format to its Int256
		///     equivalent.
		/// </summary>
		/// <param name="value">A string that contains a number to convert.</param>
		/// <param name="style">A bitwise combination of the enumeration values that specify the permitted format of value.</param>
		/// <param name="provider">An object that provides culture-specific formatting information about value.</param>
		/// <returns>A value that is equivalent to the number specified in the value parameter.</returns>
		public static UInt256 Parse (string value, NumberStyles style, IFormatProvider provider)
			{
			UInt256 result;

			if (value == null)
				throw new ArgumentException (null, "value");

			if (!TryParse (value, style, provider, out result))
				throw new FormatException ();

			return result;
			}

		/// <summary>
		///     Tries to convert the string representation of a number to its UInt256 equivalent, and returns a value that indicates
		///     whether the conversion succeeded..
		/// </summary>
		/// <param name="value">The string representation of a number.</param>
		/// <param name="result">
		///     When this method returns, contains the UInt256 equivalent to the number that is contained in value,
		///     or UInt256.Zero if the conversion failed. This parameter is passed uninitialized.
		/// </param>
		/// <returns>
		///     true if the value parameter was converted successfully; otherwise, false.
		/// </returns>
		public static bool TryParse (string value, out UInt256 result)
			{
			return TryParse (value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		/// <summary>
		///     Tries to convert the string representation of a number in a specified style and culture-specific format to its
		///     UInt256 equivalent, and returns a value that indicates whether the conversion succeeded..
		/// </summary>
		/// <param name="value">
		///     The string representation of a number. The string is interpreted using the style specified by
		///     style.
		/// </param>
		/// <param name="style">
		///     A bitwise combination of enumeration values that indicates the style elements that can be present
		///     in value. A typical value to specify is NumberStyles.Integer.
		/// </param>
		/// <param name="provider">An object that supplies culture-specific formatting information about value.</param>
		/// <param name="result">
		///     When this method returns, contains the UInt256 equivalent to the number that is contained in value,
		///     or UInt256.Zero if the conversion failed. This parameter is passed uninitialized.
		/// </param>
		/// <returns>true if the value parameter was converted successfully; otherwise, false.</returns>
		public static bool TryParse (string value, NumberStyles style, IFormatProvider provider, out UInt256 result)
			{
			var nfi = provider == null ? NumberFormatInfo.CurrentInfo : NumberFormatInfo.GetInstance (provider);

			result = Zero;
			if (StringEx.IsNullOrWhiteSpace (value))
				return false;

			if (style.HasFlag (NumberStyles.AllowLeadingWhite))
				value = value.TrimStart ();

			if (style.HasFlag (NumberStyles.AllowTrailingWhite))
				value = value.TrimEnd ();

			if (value.StartsWith ("x", StringComparison.OrdinalIgnoreCase))
				{
				if (style.HasFlag (NumberStyles.AllowHexSpecifier))
					throw new FormatException ("x not allowed with AllowHexSpecifier");

				style |= NumberStyles.AllowHexSpecifier;
				value = value.Substring (1);
				}
			else if (value.StartsWith ("0x", StringComparison.OrdinalIgnoreCase))
				{
				if (style.HasFlag (NumberStyles.AllowHexSpecifier))
					throw new FormatException ("x not allowed with AllowHexSpecifier");

				style |= NumberStyles.AllowHexSpecifier;
				value = value.Substring (2);
				}

			return style.HasFlag (NumberStyles.AllowHexSpecifier) ? TryParseHex (value, out result) : TryParseNum (value, style, nfi, out result);
			}

		private static bool TryParseHex (string value, out UInt256 result)
			{
			value = value.TrimStart ('0');
			if (value.Length > 64)
				throw new OverflowException ();

			result = Zero;
			int pos = 0;
			for (int i = value.Length - 1; i >= 0; i--)
				{
				char ch = value[i];
				ulong bch;
				if ((ch >= '0') && (ch <= '9'))
					bch = (ulong)(ch - '0');
				else if ((ch >= 'A') && (ch <= 'F'))
					bch = (ulong)(ch - 'A' + 10);
				else if ((ch >= 'a') && (ch <= 'f'))
					bch = (ulong)(ch - 'a' + 10);
				else
					return false;

				if (pos < 64)
					result._d |= bch << pos;
				else if (pos < 128)
					result._c |= bch << pos;
				else if (pos < 192)
					result._b |= bch << pos;
				else if (pos < 256)
					result._a |= bch << pos;
				pos += 4;
				}
			return true;
			}

		private static bool TryParseNum (string value, NumberStyles style, NumberFormatInfo nfi, out UInt256 result)
			{
			result = Zero;
			bool isNegative = false;
			bool hasValue = false;

			if (style.HasFlag (NumberStyles.AllowLeadingSign))
				{
				if (nfi != null)
					{
					if (value.StartsWith (nfi.PositiveSign))
						value = value.Substring (nfi.PositiveSign.Length);
					else if (value.StartsWith (nfi.NegativeSign))
						{
						value = value.Substring (nfi.NegativeSign.Length);
						isNegative = true;
						}
					}
				else if (value[0] == '+')
					value = value.Substring (1);
				else if (value[0] == '-')
					{
					value = value.Substring (1);
					isNegative = true;
					}
				}

			for (int ix = 0; ix < value.Length; ++ix)
				{
				char ch = value[ix];

				if ((ch >= '0') && (ch <= '9'))
					{
					var b = (byte)(ch - '0');
					hasValue = true;

					if (result > MaxTest || (result == MaxTest && b != 0))
						throw new OverflowException ();

					result = 10 * result;
					result += b;
					}
				else if (style.HasFlag (NumberStyles.AllowTrailingSign))
					{
					if (nfi != null)
						{
						if (value.Substring (ix) == nfi.PositiveSign)
							break;

						if (value.Substring (ix) == nfi.NegativeSign)
							{
							isNegative = true;
							break;
							}

						return false;
						}

					if (ix == value.Length - 1 && (ch == '+' || ch == '-'))
						{
						isNegative = ch == '-';
						break;
						}

					return false;
					}
				else
					return false;
				}

			if (!hasValue)
				return false;

			if (isNegative)
				result.Negate ();

			return true;
			}

		/// <summary>
		///     Converts the value of this instance to an <see cref="T:System.Object" /> of the specified
		///     <see cref="T:System.Type" /> that has an equivalent value, using the specified culture-specific formatting
		///     information.
		/// </summary>
		/// <param name="conversionType">The <see cref="T:System.Type" /> to which the value of this instance is converted.</param>
		/// <param name="provider">
		///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies
		///     culture-specific formatting information.
		/// </param>
		/// <param name="asLittleEndian">As little endian.</param>
		/// <returns>
		///     An <see cref="T:System.Object" /> instance of type <paramref name="conversionType" /> whose value is equivalent to
		///     the value of this instance.
		/// </returns>
		public object ToType (Type conversionType, IFormatProvider provider, bool asLittleEndian)
			{
			object value;
			if (TryConvert (conversionType, provider, asLittleEndian, out value))
				return value;

			throw new InvalidCastException ();
			}

		/// <summary>
		///     Compares the current instance with another object of the same type and returns an integer that indicates whether
		///     the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		///     A value that indicates the relative order of the objects being compared. The return value has these meanings: Value
		///     Meaning Less than zero This instance is less than <paramref name="obj" />. Zero This instance is equal to
		///     <paramref name="obj" />. Greater than zero This instance is greater than <paramref name="obj" />.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">
		///     <paramref name="obj" /> is not the same type as this instance.
		/// </exception>
		int IComparable.CompareTo (object obj)
			{
			return Compare (this, obj);
			}

		/// <summary>
		///     Compares two UInt256 values and returns an integer that indicates whether the first value is less than, equal to, or
		///     greater than the second value.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>A signed integer that indicates the relative values of left and right, as shown in the following table.</returns>
		public static int Compare (UInt256 left, object right)
			{
			if (right is UInt256)
				return Compare (left, (UInt256)right);

			// NOTE: this could be optimized type per type
			if (right is bool)
				return Compare (left, new UInt256 ((bool)right));

			if (right is byte)
				return Compare (left, new UInt256 ((byte)right));

			if (right is char)
				return Compare (left, new UInt256 ((char)right));

			if (right is decimal)
				return Compare (left, new UInt256 ((decimal)right));

			if (right is double)
				return Compare (left, new UInt256 ((double)right));

			if (right is short)
				return Compare (left, new UInt256 ((short)right));

			if (right is int)
				return Compare (left, new UInt256 ((int)right));

			if (right is long)
				return Compare (left, new UInt256 ((long)right));

			if (right is sbyte)
				return Compare (left, new UInt256 ((sbyte)right));

			if (right is float)
				return Compare (left, new UInt256 ((float)right));

			if (right is ushort)
				return Compare (left, new UInt256 ((ushort)right));

			if (right is uint)
				return Compare (left, new UInt256 ((uint)right));

			if (right is ulong)
				return Compare (left, new UInt256 ((ulong)right));

			var bytes = right as byte[];
			if ((bytes != null) && (bytes.Length == 32))
				{
				// TODO: ensure endian.
				return Compare (left, bytes.ToInt256 (0));
				}

			if (right is Guid)
				return Compare (left, new UInt256 ((Guid)right));

			throw new ArgumentException ();
			}

		/// <summary>
		///     Compares two 256-bit unsigned integer values and returns an integer that indicates whether the first value is less
		///     than, equal to, or greater than the second value.
		/// </summary>
		/// <param name="left">The first value to compare.</param>
		/// <param name="right">The second value to compare.</param>
		/// <returns>
		///     A signed number indicating the relative values of this instance and value.
		/// </returns>
		public static int Compare (UInt256 left, UInt256 right)
			{
			if (left.Sign == 0 && right.Sign == 0)
				return 0;

			if (left._a != right._a)
				return left._a.CompareTo (right._a);
			if (left._b != right._b)
				return left._b.CompareTo (right._b);
			if (left._c != right._c)
				return left._c.CompareTo (right._c);

			return left._d.CompareTo (right._d);
			}

		/// <summary>
		///     Compares this instance to a specified 256-bit unsigned integer and returns an indication of their relative values.
		/// </summary>
		/// <param name="value">An integer to compare.</param>
		/// <returns>A signed number indicating the relative values of this instance and value.</returns>
		public int CompareTo (UInt256 value)
			{
			return Compare (this, value);
			}

#if !NETCF
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
		private void Not ()
			{
			_a = ~_a;
			_b = ~_b;
			_c = ~_c;
			_d = ~_d;
			}

#if !NETCF
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
		private void Negate ()
			{
			Not ();
			this++;
			}

		/// <summary>
		///     Negates a specified UInt256 value.
		/// </summary>
		/// <param name="value">The value to negate.</param>
		/// <returns>The result of the value parameter multiplied by negative one (-1).</returns>
		public static UInt256 Negate (UInt256 value)
			{
			value.Negate ();
			return value;
			}

		/// <summary>
		///     Gets the absolute value this object.
		/// </summary>
		/// <returns>The absolute value.</returns>
		public UInt256 ToAbs ()
			{
			return Abs (this);
			}

		/// <summary>
		///     Gets the absolute value of an Int256 object.
		/// </summary>
		/// <param name="value">A number.</param>
		/// <returns>
		///     The absolute value.
		/// </returns>
		public static UInt256 Abs (UInt256 value)
			{
			return value;
			}

		/// <summary>
		///     Adds two UInt256 values and returns the result.
		/// </summary>
		/// <param name="left">The first value to add.</param>
		/// <param name="right">The second value to add.</param>
		/// <returns>The sum of left and right.</returns>
		public static UInt256 Add (UInt256 left, UInt256 right)
			{
			return left + right;
			}

		/// <summary>
		///     Subtracts one UInt256 value from another and returns the result.
		/// </summary>
		/// <param name="left">The value to subtract from (the minuend).</param>
		/// <param name="right">The value to subtract (the subtrahend).</param>
		/// <returns>The result of subtracting right from left.</returns>
		public static UInt256 Subtract (UInt256 left, UInt256 right)
			{
			return left - right;
			}

		/// <summary>
		///     Divides one UInt256 value by another and returns the result.
		/// </summary>
		/// <param name="dividend">The value to be divided.</param>
		/// <param name="divisor">The value to divide by.</param>
		/// <returns>The quotient of the division.</returns>
		public static UInt256 Divide (UInt256 dividend, UInt256 divisor)
			{
			UInt256 integer;
			return DivRem (dividend, divisor, out integer);
			}

		/// <summary>
		///     Performs integer division on two UInt256 values and returns the remainder.
		/// </summary>
		/// <param name="dividend">The value to be divided.</param>
		/// <param name="divisor">The value to divide by.</param>
		/// <returns>The remainder after dividing dividend by divisor.</returns>
		public static UInt256 Remainder (UInt256 dividend, UInt256 divisor)
			{
			UInt256 remainder;
			DivRem (dividend, divisor, out remainder);
			return remainder;
			}

		/// <summary>
		///     Divides one UInt256 value by another, returns the result, and returns the remainder in an output parameter.
		/// </summary>
		/// <param name="dividend">The value to be divided.</param>
		/// <param name="divisor">The value to divide by.</param>
		/// <param name="remainder">
		///     When this method returns, contains an UInt256 value that represents the remainder from the
		///     division. This parameter is passed uninitialized.
		/// </param>
		/// <returns>
		///     The quotient of the division.
		/// </returns>
		public static UInt256 DivRem (UInt256 dividend, UInt256 divisor, out UInt256 remainder)
			{
			if (divisor.Sign == 0)
				throw new DivideByZeroException ();
			if (dividend.Sign == 0)
				{
				remainder = Zero;
				return Zero;
				}

			uint[] quotient;
			uint[] rem;
			MathUtils.DivModUnsigned (dividend.ToUIn32Array (), divisor.ToUIn32Array (), out quotient, out rem);
			remainder = new UInt256 (1, rem);
			return new UInt256 (1, quotient);
			}

		/// <summary>
		///     Converts an UInt256 value to an unsigned long array.
		/// </summary>
		/// <returns>
		///     The value of the current Int256 object converted to an array of unsigned integers.
		/// </returns>
		public ulong[] ToUIn64Array ()
			{
			return new[] {_d, _c, _b, _a};
			}

		/// <summary>
		///     Converts an Int256 value to an unsigned integer array.
		/// </summary>
		/// <returns>The value of the current UInt256 object converted to an array of unsigned integers.</returns>
		public uint[] ToUIn32Array ()
			{
			var ints = new uint[8];
			ulong[] ulongs = ToUIn64Array ();
			Buffer.BlockCopy (ulongs, 0, ints, 0, 32);
			return ints;
			}

		/// <summary>
		///     Returns the product of two UInt256 values.
		/// </summary>
		/// <param name="left">The first number to multiply.</param>
		/// <param name="right">The second number to multiply.</param>
		/// <returns>The product of the left and right parameters.</returns>
		public static UInt256 Multiply (UInt256 left, UInt256 right)
			{
			if (left.Sign == 0 || right.Sign == 0)
				return Zero;

			uint[] xInts = left.ToUIn32Array ();
			uint[] yInts = right.ToUIn32Array ();
			var mulInts = new uint[16];

			for (int i = 0; i < xInts.Length; i++)
				{
				int index = i;
				ulong remainder = 0;
				foreach (uint yi in yInts)
					{
					remainder = remainder + (ulong)xInts[i] * yi + mulInts[index];
					mulInts[index++] = (uint)remainder;
					remainder = remainder >> 32;
					}

				while (remainder != 0)
					{
					remainder += mulInts[index];
					mulInts[index++] = (uint)remainder;
					remainder = remainder >> 32;
					}
				}
			return new UInt256 (1, mulInts);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="System.Boolean" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">if set to <c>true</c> [value].</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator UInt256 (bool value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an implicit conversion from <see cref="System.Byte" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static implicit operator UInt256 (byte value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an implicit conversion from <see cref="System.Char" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static implicit operator UInt256 (char value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="System.Decimal" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator UInt256 (decimal value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="System.Double" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator UInt256 (double value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="System.Int16" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator UInt256 (short value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="System.Int32" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator UInt256 (int value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="System.Int64" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator UInt256 (long value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="System.SByte" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator UInt256 (sbyte value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="System.Single" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator UInt256 (float value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an implicit conversion from <see cref="System.UInt16" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static implicit operator UInt256 (ushort value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an implicit conversion from <see cref="System.UInt32" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static implicit operator UInt256 (uint value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an implicit conversion from <see cref="System.UInt64" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static implicit operator UInt256 (ulong value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="Int128" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator UInt256 (Int128 value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an implicit conversion from <see cref="UInt128" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static implicit operator UInt256 (UInt128 value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="Int256" /> to <see cref="UInt256" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator UInt256 (Int256 value)
			{
			return new UInt256 (value);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.Boolean" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator bool (UInt256 value)
			{
			return value.Sign != 0;
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.Byte" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator byte (UInt256 value)
			{
			if (value.Sign == 0)
				return 0;

			if (value > byte.MaxValue)
				throw new OverflowException ();

			return (byte)value._d;
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.SByte" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator sbyte (UInt256 value)
			{
			if (value.Sign == 0)
				return 0;

			if (value > (UInt128)sbyte.MaxValue)
				throw new OverflowException ();

			return (sbyte)value._d;
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.Char" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator char (UInt256 value)
			{
			if (value.Sign == 0)
				return (char)0;

			if (value > char.MaxValue)
				throw new OverflowException ();

			return (char)value._d;
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.Decimal" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator decimal (UInt256 value)
			{
			if (value.Sign == 0)
				return 0;

			if (value > (UInt256)decimal.MaxValue)
				throw new OverflowException ();

			return new decimal ((int)(value._d & 0xFFFFFFFF), (int)(value._d >> 32), (int)(value._c & 0xFFFFFFFF), false, 0);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.Double" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator double (UInt256 value)
			{
			if (value.Sign == 0)
				return 0;

			double d;
			NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;
			if (!DoubleEx.TryParse (value.ToString (nfi), NumberStyles.Number, nfi, out d))
				throw new OverflowException ();

			return d;
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.Single" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator float (UInt256 value)
			{
			if (value.Sign == 0)
				return 0;

			float f;
			NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;
			if (!SingleEx.TryParse (value.ToString (nfi), NumberStyles.Number, nfi, out f))
				throw new OverflowException ();

			return f;
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.Int16" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator short (UInt256 value)
			{
			if (value.Sign == 0)
				return 0;

			if (value > (UInt256)short.MaxValue)
				throw new OverflowException ();

			return (short)value._d;
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.Int32" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator int (UInt256 value)
			{
			if (value.Sign == 0)
				return 0;

			if (value > (UInt256)int.MaxValue)
				throw new OverflowException ();

			return ((int)value._d);
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.Int64" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator long (UInt256 value)
			{
			if (value.Sign == 0)
				return 0;

			if (value > (UInt256)long.MaxValue)
				throw new OverflowException ();

			return (long)value._d;
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.UInt32" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator uint (UInt256 value)
			{
			if (value.Sign == 0)
				return 0;

			if (value > uint.MaxValue)
				throw new OverflowException ();

			return (uint)value._d;
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.UInt16" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator ushort (UInt256 value)
			{
			if (value.Sign == 0)
				return 0;

			if (value > ushort.MaxValue)
				throw new OverflowException ();

			return (ushort)value._d;
			}

		/// <summary>
		///     Performs an explicit conversion from <see cref="UInt256" /> to <see cref="System.UInt64" />.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the conversion.
		/// </returns>
		public static explicit operator ulong (UInt256 value)
			{
			if (value > ushort.MaxValue)
				throw new OverflowException ();

			return value._d;
			}

		/// <summary>
		///     Implements the operator &gt;.
		/// </summary>
		/// <param name="left">The x.</param>
		/// <param name="right">The y.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static bool operator > (UInt256 left, UInt256 right)
			{
			return Compare (left, right) > 0;
			}

		/// <summary>
		///     Implements the operator &lt;.
		/// </summary>
		/// <param name="left">The x.</param>
		/// <param name="right">The y.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static bool operator < (UInt256 left, UInt256 right)
			{
			return Compare (left, right) < 0;
			}

		/// <summary>
		///     Implements the operator &gt;=.
		/// </summary>
		/// <param name="left">The x.</param>
		/// <param name="right">The y.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static bool operator >= (UInt256 left, UInt256 right)
			{
			return Compare (left, right) >= 0;
			}

		/// <summary>
		///     Implements the operator &lt;=.
		/// </summary>
		/// <param name="left">The x.</param>
		/// <param name="right">The y.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static bool operator <= (UInt256 left, UInt256 right)
			{
			return Compare (left, right) <= 0;
			}

		/// <summary>
		///     Implements the operator !=.
		/// </summary>
		/// <param name="left">The x.</param>
		/// <param name="right">The y.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static bool operator != (UInt256 left, UInt256 right)
			{
			return Compare (left, right) != 0;
			}

		/// <summary>
		///     Implements the operator ==.
		/// </summary>
		/// <param name="left">The x.</param>
		/// <param name="right">The y.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static bool operator == (UInt256 left, UInt256 right)
			{
			return Compare (left, right) == 0;
			}

		/// <summary>
		///     Implements the operator +.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static UInt256 operator + (UInt256 value)
			{
			return value;
			}

		/// <summary>
		///     Implements the operator -.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static UInt256 operator - (UInt256 value)
			{
			return Negate (value);
			}

		/// <summary>
		///     Implements the operator +.
		/// </summary>
		/// <param name="left">The x.</param>
		/// <param name="right">The y.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static UInt256 operator + (UInt256 left, UInt256 right)
			{
			left._a += right._a;
			left._b += right._b;
			if (left._b < right._b)
				left._a++;
			left._c += right._c;
			if (left._c < right._c)
				{
				left._b++;
				if (left._b < left._b - 1)
					left._a++;
				}
			left._d += right._d;
			if (left._d < right._d)
				{
				left._c++;
				if (left._c < left._c - 1)
					{
					left._b++;
					if (left._b < left._b - 1)
						left._a++;
					}
				}

			return left;
			}

		/// <summary>
		///     Implements the operator -.
		/// </summary>
		/// <param name="left">The x.</param>
		/// <param name="right">The y.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static UInt256 operator - (UInt256 left, UInt256 right)
			{
			return left + -right;
			}

		/// <summary>
		///     Implements the operator %.
		/// </summary>
		/// <param name="dividend">The dividend.</param>
		/// <param name="divisor">The divisor.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static UInt256 operator % (UInt256 dividend, UInt256 divisor)
			{
			return Remainder (dividend, divisor);
			}

		/// <summary>
		///     Implements the operator /.
		/// </summary>
		/// <param name="dividend">The dividend.</param>
		/// <param name="divisor">The divisor.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static UInt256 operator / (UInt256 dividend, UInt256 divisor)
			{
			return Divide (dividend, divisor);
			}

		/// <summary>
		///     Implements the operator *.
		/// </summary>
		/// <param name="left">The x.</param>
		/// <param name="right">The y.</param>
		/// <returns>
		///     The result of the operator.
		/// </returns>
		public static UInt256 operator * (UInt256 left, UInt256 right)
			{
			return Multiply (left, right);
			}

		/// <summary>
		///     Implements the operator &gt;&gt;.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="shift">The shift.</param>
		/// <returns>The result of the operator.</returns>
		public static UInt256 operator >> (UInt256 value, int shift)
			{
			if (shift == 0)
				return value;

			ulong[] bits = MathUtils.ShiftRight (value.ToUIn64Array (), shift);
			value._a = bits[3];
			value._b = bits[2];
			value._c = bits[1];
			value._d = bits[0]; //lo is stored in array entry 0

			return value;
			}

		/// <summary>
		///     Implements the operator &lt;&lt;.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="shift">The shift.</param>
		/// <returns>The result of the operator.</returns>
		public static UInt256 operator << (UInt256 value, int shift)
			{
			if (shift == 0)
				return value;

			ulong[] bits = MathUtils.ShiftLeft (value.ToUIn64Array (), shift);
			value._a = bits[3];
			value._b = bits[2];
			value._c = bits[1];
			value._d = bits[0]; //lo is stored in array entry 0

			return value;
			}

		/// <summary>
		///     Implements the operator |.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static UInt256 operator | (UInt256 left, UInt256 right)
			{
			if (left.Sign == 0)
				return right;

			if (right.Sign == 0)
				return left;

			left._a |= right._a;
			left._b |= right._b;
			left._c |= right._c;
			left._d |= right._d;
			return left;
			}

		/// <summary>
		///     Implements the operator &amp;.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static UInt256 operator & (UInt256 left, UInt256 right)
			{
			if (left.Sign == 0 || right.Sign == 0)
				return Zero;

			left._a &= right._a;
			left._b &= right._b;
			left._c &= right._c;
			left._d &= right._d;
			return left;
			}

		/// <summary>
		///     Implements the operator ^.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>The result of the operator.</returns>
		public static UInt256 operator ^ (UInt256 left, UInt256 right)
			{
			if (left.Sign == 0)
				return right;

			if (right.Sign == 0)
				return left;

			left._a ^= right._a;
			left._b ^= right._b;
			left._c ^= right._c;
			left._d ^= right._d;
			return left;
			}

		/// <summary>
		///     Implements the operator ~.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operator.</returns>
		public static UInt256 operator ~ (UInt256 value)
			{
			return new UInt256 (~value._a, ~value._b, ~value._c, ~value._d);
			}

		/// <summary>
		///     Implements the operator ++.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operator.</returns>
		public static UInt256 operator ++ (UInt256 value)
			{
			return value + 1;
			}

		/// <summary>
		///     Implements the operator --.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>The result of the operator.</returns>
		public static UInt256 operator -- (UInt256 value)
			{
			return value - 1;
			}

		#region IConvertible Members

		TypeCode IConvertible.GetTypeCode ()
			{
			return TypeCode.Object;
			}

		bool IConvertible.ToBoolean (IFormatProvider provider)
			{
			return (bool)this;
			}

		byte IConvertible.ToByte (IFormatProvider provider)
			{
			return (byte)this;
			}

		char IConvertible.ToChar (IFormatProvider provider)
			{
			return (char)this;
			}

		DateTime IConvertible.ToDateTime (IFormatProvider provider)
			{
			throw new InvalidCastException ();
			}

		decimal IConvertible.ToDecimal (IFormatProvider provider)
			{
			return (decimal)this;
			}

		double IConvertible.ToDouble (IFormatProvider provider)
			{
			return (double)this;
			}

		short IConvertible.ToInt16 (IFormatProvider provider)
			{
			return (short)this;
			}

		int IConvertible.ToInt32 (IFormatProvider provider)
			{
			return (int)this;
			}

		long IConvertible.ToInt64 (IFormatProvider provider)
			{
			return (long)this;
			}

		sbyte IConvertible.ToSByte (IFormatProvider provider)
			{
			return (sbyte)this;
			}

		float IConvertible.ToSingle (IFormatProvider provider)
			{
			return (float)this;
			}

		string IConvertible.ToString (IFormatProvider provider)
			{
			return this.ToString (null, provider);
			}

		object IConvertible.ToType (Type conversionType, IFormatProvider provider)
			{
			return ToType (conversionType, provider, true);
			}

		ushort IConvertible.ToUInt16 (IFormatProvider provider)
			{
			return (ushort)this;
			}

		uint IConvertible.ToUInt32 (IFormatProvider provider)
			{
			return (uint)this;
			}

		ulong IConvertible.ToUInt64 (IFormatProvider provider)
			{
			return (ulong)this;
			}

		#endregion
		}
	}
