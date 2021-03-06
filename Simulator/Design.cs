﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Traffic_Simulator
{
    /// <summary> 
    /// Traffic Simulator, Design Controller. This Class is used as a controller
    /// to manage adding and removing design units to and from the design grid.
    /// When it is first called it creates a newDesign form, that it uses to ask the
    /// user to specify the design parameters (Name & Grid size). Once specified,
    /// it then creates a new design Form, given the specified user parameters.
    /// </summary>

    public class Design
    {
        /// <summary>
        /// DesignName variable
        /// </summary>
        private String designName;
        /// <summary>
        /// Parent MainGuiForm
        /// </summary>
        private MainGuiForm parent;
        /// <summary>
        /// WorldBounds Point
        /// </summary>
        private Point worldBounds;
        /// <summary>
        /// NewDesignForm DesignGuiForm
        /// </summary>
        private DesignGuiForm newDesignForm;
        /// <summary>
        /// CreateNewDesign newDesign
        /// </summary>
        private newDesign createNewDesign;
        /// <summary>
        /// DesignUnitArray array
        /// </summary>
        private RoadUnit[,] designUnitArray;
        /// <summary>
        /// RoadUnitStartingId integer
        /// </summary>
        private int roadUnitStartingId = 0;

        /// <summary> 
        /// Design Constructer. Initalize the designGuiForm.
        /// Is passed the parent form [mainGuiForm] as a 
        /// parameter so among other things the name of the
        /// form can be changed to match the design name.
        /// </summary> 
        /// <param name="parent">MainGuiForm</param>

        public Design(MainGuiForm parent)
        {
            this.parent = parent;
            System.Console.WriteLine("Called new DesignGui!");
            createNewDesign = new newDesign(this);
            createNewDesign.Show();
        }
         /// <summary> 
         /// DesignParameterSet method.
         /// Is used when the newDesign form has finished
         /// validating its inputted parameters, in order
         /// to pass the design parameters entered back
         /// to the design.
         /// </summary> 

        public void designParameterSet()
        {
            this.worldBounds = createNewDesign.worldBounds;
            this.designName = createNewDesign.designName;
            System.Console.WriteLine("Design name: " + designName);
            System.Console.WriteLine("Set world bounds: " + worldBounds);
            createNewDesign.Dispose();
            newDesignForm = new DesignGuiForm(worldBounds, designName, this);
            System.Console.WriteLine("Main Display name changed from: " + parent.Text);
            parent.Text = designName;
            System.Console.WriteLine(" to: " + parent.Text);
            parent.Refresh();
            parent.Show();
            newDesignForm.MdiParent = parent;
            newDesignForm.WindowState = FormWindowState.Maximized;
            newDesignForm.Show();
            newDesignForm.Refresh();
            designUnitArray = new RoadUnit[worldBounds.Y, worldBounds.X];
        }

        /// <summary> 
        /// AddRoadUnit method. Is used by the design Form, when the user wishes
        /// to add/place another unit onto the design form's grid area. This method
        /// checks if there is any other unit which already occupies that tile, as well
        /// as any other tiles within it immediate footprint.
        /// </summary>
        /// <param name="roadUnit">RoadUnit</param>

        public bool addRoadUnit(RoadUnit roadUnit)
        {
            if (designUnitArray[roadUnit.gridValue.Y, roadUnit.gridValue.X] == null)
            {
                if (roadUnit.NumOfLanes > 1)
                {
                    int count = roadUnit.NumOfLanes-1;
                    while (count >= 0)
                    {
                        if (designUnitArray[roadUnit.gridValue.Y, roadUnit.gridValue.X + count] != null)
                        {
                            System.Console.WriteLine("Road unit: id-" + designUnitArray[roadUnit.gridValue.Y, roadUnit.gridValue.X + count].UnitID + " allready occupies [" + roadUnit.gridValue.Y + "," + (roadUnit.gridValue.X+count) + "]");
                            return false;
                        }
                        else
                            count--;
                    }
                    roadUnit.UnitID = roadUnitStartingId;
                    roadUnitStartingId++;
                    count = 0;
                    while (count <= (roadUnit.NumOfLanes-1) )
                    {
                        designUnitArray[roadUnit.gridValue.Y, roadUnit.gridValue.X+count] = roadUnit;
                        if (count == 0)
                        {
                            System.Console.Write("Added unit [ id-" + roadUnit.UnitID + "] to posistions: {" + roadUnit.gridValue.Y + "," + (roadUnit.gridValue.X + count)+"},");
                        }
                        else
                            System.Console.Write("{" + roadUnit.gridValue.Y + "," + (roadUnit.gridValue.X + count) + "},");
                        count++;
                    }
                    System.Console.WriteLine("");
                    return true;
                }
                else
                {
                    roadUnit.UnitID = roadUnitStartingId;
                    roadUnitStartingId++;
                    designUnitArray[roadUnit.gridValue.Y, roadUnit.gridValue.X] = roadUnit;
                    System.Console.WriteLine("Added unit [ id-"+roadUnit.UnitID+" ] to posistion: " + roadUnit.gridValue.Y + "," + roadUnit.gridValue.X);
                    return true;
                }
            }
            else
            {
                System.Console.WriteLine("Error adding unit: [ Row:" + roadUnit.gridValue.Y + ", Coloumn:" + roadUnit.gridValue.X + "] Taken by: id-" + designUnitArray[roadUnit.gridValue.Y, roadUnit.gridValue.X].UnitID);
                return false;
            }
            return false;
        }

        /// <summary> 
        /// ModifiedUnitArray accessor method.
        /// Returns the current array of units on the grid.
        /// </summary>

        public RoadUnit[,] modifiedUnitArray
        {
            get { return designUnitArray; }
        }

        /// <summary> 
        /// AddRoadSign method. Is used by the design Form, when the user wishes
        /// to add/place a RoadSign unit onto the design form's grid area. This method
        /// checks if there is any other unit which already occupies that tile, as well
        /// as any other tiles within it immediate footprint.
        /// </summary>
        /// <param name="roadSign">RoadSign</param>

        public void addRoadSign(RoadSign roadSign)
        {

        }

        /// <summary> 
        /// AddTrafficLight method. Is used by the design Form, when the user wishes
        /// to add/place a TrafficLight unit onto the design form's grid area. This method
        /// checks if there is any other unit which already occupies that tile, as well
        /// as any other tiles within it immediate footprint.
        /// </summary>
        /// <param name="trafficLight">TrafficLight</param>

        public void addTrafficLight(TrafficLight trafficLight)
        {

        }

        /// <summary> 
        /// RemoveTrafficLight method. Is used by the design Form, when the user wishes
        /// to remove a TrafficLight from the design form's grid area. It removes the unit
        /// from all tiles under it's footprint, by specifying a starting grid position.
        /// </summary>
        /// <param name="xGrid">Integer</param>
        /// <param name="yGrid">Integer</param>

        public void removeTrafficLight(int xGrid, int yGrid)
        {

        }

        /// <summary> 
        /// RemoveRoadSign method. Is used by the design Form, when the user wishes
        /// to remove a RoadSign from the design form's grid area. It removes the unit
        /// from all tiles under it's footprint, by specifying a starting grid position.
        /// </summary>
        /// <param name="xGrid">Integer</param>
        /// <param name="yGrid">Integer</param>
        
        public void removeRoadSign(int xGrid, int yGrid)
        {

        }

        /// <summary> 
        /// RemoveRoadUnit method. Is used by the design Form, when the user wishes
        /// to remove a RoadUnit from the design form's grid area. It removes the unit
        /// from all tiles under it's footprint, by specifying a starting grid position.
        /// </summary>
        /// <param name="xGrid">Integer</param>
        /// <param name="yGrid">Integer</param>
        
        public void removeRoadUnit(int xGrid, int yGrid)
        {

        }

        /// <summary> 
        /// SaveDesign method. This is called when the user wishes to save the
        /// current design.
        /// </summary>
        /// <param name="dsign">Design</param>
        
        public void saveDesign(Design dsign)
        {

        }

        /// <summary> 
        /// OpenDesign method. This is called when the user wishes to open
        /// an existing design.
        /// </summary>
        /// <param name="designID">String</param>
        
        public Design openDesign(String designID)
        {
            return null;
        }
    }
}
