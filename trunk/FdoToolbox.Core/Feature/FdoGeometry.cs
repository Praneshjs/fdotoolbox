#region LGPL Header
// Copyright (C) 2009, Jackie Ng
// http://code.google.com/p/fdotoolbox, jumpinjackie@gmail.com
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
//
// See license.txt for more/additional licensing information
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.FDO.Geometry;

namespace FdoToolbox.Core.Feature
{
    /// <summary>
    /// <see cref="IGeometry"/> decorator
    /// </summary>
    public class FdoGeometry : IGeometry
    {
        private IGeometry _geom;

        /// <summary>
        /// Initializes a new instance of the <see cref="FdoGeometry"/> class.
        /// </summary>
        /// <param name="geom">The geom.</param>
        public FdoGeometry(IGeometry geom)
        {
            _geom = geom;
        }

        /// <summary>
        /// Gets the dervied type
        /// </summary>
        /// <value>The type of the derived.</value>
        public OSGeo.FDO.Common.GeometryType DerivedType
        {
            get { return _geom.DerivedType; }
        }

        /// <summary>
        /// Gets the dimensionality.
        /// </summary>
        /// <value>The dimensionality.</value>
        public int Dimensionality
        {
            get { return _geom.Dimensionality; }
        }

        /// <summary>
        /// Gets the envelope.
        /// </summary>
        /// <value>The envelope.</value>
        public IEnvelope Envelope
        {
            get { return _geom.Envelope; }
        }

        /// <summary>
        /// Gets the FGF text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return _geom.Text; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _geom.Dispose();
        }

        /// <summary>
        /// Gets the decorated/wrapped geometry
        /// </summary>
        public IGeometry InternalGeometry
        {
            get { return _geom; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            //This is the whole reason for having a decorator. When in a DataTable, the native IGeometry's ToString() shows nothing, when it should be really showing the FGF text
            return _geom.Text; 
        }
    }
}
