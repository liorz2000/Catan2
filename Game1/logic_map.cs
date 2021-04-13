using System;
using System.Collections.Generic;
using System.Text;

//dict.ContainsKey(key);

namespace Game1
{
    class logic_map
    {
        public Dictionary<(int, int, int), Edge> edges;
        public Dictionary<(int, int), Cell> cells;
        public Dictionary<(int, int, bool), Vertex> vertices;
        
        public int num_of_land_cells;
        public Dictionary<string, int> reasors_balance;

        public logic_map(HashSet <(int, int, string)> cells_with_base_type, Dictionary<(int, int, int) , string> ports = null)
        {
            // base type map, with ports!
            num_of_land_cells = 0;
            cells = new Dictionary<(int, int), Cell>();
            edges = new Dictionary<(int, int, int), Edge>();
            vertices = new Dictionary<(int, int, bool), Vertex>();
            
            foreach ((int,int,string) cell in cells_with_base_type )
            
            {
                // Add the cells
                cells.Add((cell.Item1, cell.Item2), new Cell(cell.Item1, cell.Item2, cell.Item3));
                if (cell.Item3 == "land")
                {
                    num_of_land_cells += 1;
                }

                //Add the edges
                (int, int,int) e1 = (cell.Item1, cell.Item2, 1);
                (int, int, int) e3 = (cell.Item1, cell.Item2 + 1, 9);
                (int, int, int) e5 = (cell.Item1, cell.Item2, 5);
                (int, int, int) e7 = (cell.Item1 + 1, cell.Item2 - 1, 1);
                (int, int, int) e9 = (cell.Item1, cell.Item2, 9);
                (int, int, int) e11 = (cell.Item1 - 1, cell.Item2, 5);

                HashSet<(int, int, int)> maybe_new_edges = new HashSet<(int, int, int)> { e1, e3, e5 , e7, e9, e11};

                foreach ((int, int ,int) edge in maybe_new_edges)
                {
                    if (!edges.ContainsKey((edge.Item1, edge.Item2, edge.Item3)))
                    {
                        edges.Add(edge, new Edge(edge.Item1, edge.Item2, edge.Item3));
                    }
                }

                //Add the vertises

                (int, int, bool) v0 = (cell.Item1, cell.Item2, true);
                (int, int, bool) v2 = (cell.Item1-1, cell.Item2+1, false);
                (int, int, bool) v4 = (cell.Item1+1, cell.Item2, true);
                (int, int, bool) v6 = (cell.Item1, cell.Item2, false);
                (int, int, bool) v8 = (cell.Item1+1, cell.Item2-1, true);
                (int, int, bool) v10 = (cell.Item1-1, cell.Item2, false);

                HashSet<(int, int, bool)> maybe_new_vertices = new HashSet<(int, int, bool)> { v0, v2, v4, v6, v8, v10 };

                foreach ((int, int, bool) vertex in maybe_new_vertices)
                {
                    if (!vertices.ContainsKey((vertex.Item1, vertex.Item2, vertex.Item3)))
                    {
                        vertices.Add(vertex, new Vertex(vertex.Item1, vertex.Item2, vertex.Item3));
                    }
                }

                


            }
            conact_V_E_C();
            Add_ports(ports);
        }

        public void conact_V_E_C()
        {
            //vertices_vote_to_edges_and_cells

            (int, int) cellii;
            (int, int, int) edgeiii;
            foreach (Vertex vertex in vertices.Values)
            {
                //fix the problem of non exsit keys!!!!!!!!
                if (vertex.is_up_or_down)
                {
                    cellii = (vertex.row, vertex.colum);
                    if (cells.ContainsKey(cellii))
                    {
                        vertex.cells.Add(cellii, cells[cellii]);
                    }

                    cellii = (vertex.row - 1, vertex.colum);
                    if (cells.ContainsKey(cellii))
                    {
                        vertex.cells.Add(cellii, cells[cellii]);
                    }

                    cellii = (vertex.row - 1, vertex.colum + 1);
                    if (cells.ContainsKey(cellii))
                    {
                        vertex.cells.Add(cellii, cells[cellii]);
                    }

                    edgeiii = (vertex.row, vertex.colum,1);
                    if (edges.ContainsKey(edgeiii))
                    {
                        vertex.edges.Add(edgeiii, edges[edgeiii]);
                    }

                    edgeiii = (vertex.row - 1, vertex.colum, 5);
                    if (edges.ContainsKey(edgeiii))
                    {
                        vertex.edges.Add(edgeiii, edges[edgeiii]);
                    }

                    edgeiii = (vertex.row - 1, vertex.colum + 1, 9);
                    if (edges.ContainsKey(edgeiii))
                    {
                        vertex.edges.Add(edgeiii, edges[edgeiii]);
                    }
                }
                else
                {
                    cellii = (vertex.row, vertex.colum);
                    if (cells.ContainsKey(cellii))
                    {
                        vertex.cells.Add(cellii, cells[cellii]);
                    }

                    cellii = (vertex.row + 1, vertex.colum);
                    if (cells.ContainsKey(cellii))
                    {
                        vertex.cells.Add(cellii, cells[cellii]);
                    }

                    cellii = (vertex.row + 1, vertex.colum - 1);
                    if (cells.ContainsKey(cellii))
                    {
                        vertex.cells.Add(cellii, cells[cellii]);
                    }

                    edgeiii = (vertex.row + 1, vertex.colum - 1, 1);
                    if (edges.ContainsKey(edgeiii))
                    {
                        vertex.edges.Add(edgeiii, edges[edgeiii]);
                    }

                    edgeiii = (vertex.row, vertex.colum, 5);
                    if (edges.ContainsKey(edgeiii))
                    {
                        vertex.edges.Add(edgeiii, edges[edgeiii]);
                    }

                    edgeiii = (vertex.row + 1, vertex.colum, 9);
                    if (edges.ContainsKey(edgeiii))
                    {
                        vertex.edges.Add(edgeiii, edges[edgeiii]);
                    }

                
                }
            }

            //cells_vote_to_vertices

            foreach (Cell cell in cells.Values)
            {
                cell.vertices.Add((cell.row, cell.colum, true), vertices[(cell.row, cell.colum, true)]);
                cell.vertices.Add((cell.row - 1, cell.colum + 1, false), vertices[(cell.row - 1, cell.colum + 1, false)]);
                cell.vertices.Add((cell.row + 1, cell.colum, true), vertices[(cell.row + 1, cell.colum, true)]);
                cell.vertices.Add((cell.row, cell.colum, false), vertices[(cell.row, cell.colum, false)]);
                cell.vertices.Add((cell.row + 1, cell.colum - 1, true), vertices[(cell.row + 1, cell.colum - 1, true)]);
                cell.vertices.Add((cell.row - 1, cell.colum, false), vertices[(cell.row - 1, cell.colum, false)]);
                /*
                (int, int, bool) v0 = (cell.Item1, cell.Item2, true);
                (int, int, bool) v2 = (cell.Item1-1, cell.Item2+1, false);
                (int, int, bool) v4 = (cell.Item1+1, cell.Item2, true);
                (int, int, bool) v6 = (cell.Item1, cell.Item2, false);
                (int, int, bool) v8 = (cell.Item1+1, cell.Item2-1, true);
                (int, int, bool) v10 = (cell.Item1-1, cell.Item2, false);
                 */
            }

            //edges_vote_to_vertiecs
            foreach (Edge edge in edges.Values)
            {
                if (edge.direction == 1)
                {
                    edge.vertices.Add((edge.row, edge.colum, true), vertices[(edge.row, edge.colum, true)]);
                    edge.vertices.Add((edge.row - 1, edge.colum + 1, false), vertices[(edge.row - 1, edge.colum + 1, false)]);
                }
                if (edge.direction == 5)
                {
                    edge.vertices.Add((edge.row+1, edge.colum, true), vertices[(edge.row+1, edge.colum, true)]);
                    edge.vertices.Add((edge.row , edge.colum, false), vertices[(edge.row , edge.colum , false)]);
                }
                if (edge.direction == 9)
                {
                    edge.vertices.Add((edge.row + 1, edge.colum-1, true), vertices[(edge.row + 1, edge.colum-1, true)]);
                    edge.vertices.Add((edge.row-1, edge.colum, false), vertices[(edge.row-1, edge.colum, false)]);
                }
            }
        }
        public void Add_ports(Dictionary<(int, int, int), string> ports)
        {
            
        }

    }
}
