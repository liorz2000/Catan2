using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using System.Linq;

namespace Game1
{

    class GraphicMap
    {

        public double r = 2/Math.Sqrt(3);


        //public logic_map lmap = new logic_map(new HashSet<(int, int, string)>());
        public int stage = 1;
        public Dictionary<(int, int, int), Edge> edges;
        public Dictionary<(int, int), Cell> cells;
        public Dictionary<(int, int, bool), Vertex> vertices;
        public Dictionary<string, Texture2D> textures_given;

        //Information from stage1
        public int random_land_balance;
        public HashSet<(int, int)> sea_cells = new HashSet<(int, int)>();
        public HashSet<(int, int)> land_cells = new HashSet<(int, int)>();
        public HashSet<(int, int)> sea_land_cells = new HashSet<(int, int)>();
        public HashSet<(int, int)> all_cells = new HashSet<(int, int)>();

        public Dictionary<string, int> random_reasors_balance;
        public Dictionary<int, int> random_num_balance;


        public int step_size;
        public Point zero_hex_center;
        public int edge_ratio = 5;
        public Dictionary<string, Texture2D> textures;
        public Game1 game;

        //public

        //public List<Button> cell_buttons = new List<Button>();
        //public List<Button> num_buttons = new List<Button>();

        public GraphicMap(Dictionary<string, Texture2D> textures_given, HashSet<(int, int)> cells_indexes, Game1 game)
        {
            //stage1 intialization
            stage = 1;
            create_map_stage1(textures_given, cells_indexes);
            this.game = game;
        }

        public GraphicMap(Dictionary<string, Texture2D> textures_given, HashSet<(int, int)> cells_indexes,
            HashSet<(int, int)> sea_cells, HashSet<(int, int)> land_cells, HashSet<(int, int)> sea_land_cells)
        {
            //stage2 intialization
            stage = 2;
            create_map_stage1(textures_given, cells_indexes);
            foreach ((int, int) cellii in cells.Keys)
            {
                if (sea_cells.Contains(cellii))
                {
                    cells[cellii].is_have_base_type = true;
                    cells[cellii].base_type = "sea";
                }
                if (land_cells.Contains(cellii))
                {
                    cells[cellii].is_have_base_type = true;
                    cells[cellii].base_type = "land";
                }
                if (sea_land_cells.Contains(cellii))
                {
                    cells[cellii].is_have_base_type = false;
                }
            }
            
            add_edges();
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

                    edgeiii = (vertex.row, vertex.colum, 1);
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
                    edge.vertices.Add((edge.row, edge.col, true), vertices[(edge.row, edge.col, true)]);
                    edge.vertices.Add((edge.row - 1, edge.col + 1, false), vertices[(edge.row - 1, edge.col + 1, false)]);
                }
                if (edge.direction == 5)
                {
                    edge.vertices.Add((edge.row + 1, edge.col, true), vertices[(edge.row + 1, edge.col, true)]);
                    edge.vertices.Add((edge.row, edge.col, false), vertices[(edge.row, edge.col, false)]);
                }
                if (edge.direction == 9)
                {
                    edge.vertices.Add((edge.row + 1, edge.col - 1, true), vertices[(edge.row + 1, edge.col - 1, true)]);
                    edge.vertices.Add((edge.row - 1, edge.col, false), vertices[(edge.row - 1, edge.col, false)]);
                }
            }
        }

        public int get_random_sea_land_number_stage1()
        {
            int counter = 0;
            foreach((int, int) cellii in cells.Keys)
            {
                if (cells[cellii].self_button.phase_index == 3)
                {
                    counter += 1;
                }
            }
            return counter;
        }

        public int get_random_ports_number_stage2()
        {
            int counter = 0;
            foreach ((int, int, int ) edgeiii in edges.Keys)
            {
                if (edges[edgeiii].self_button.phase_index == 2)
                {
                    counter += 1;
                }
            }
            return counter;
        }

        public void add_cell_stage1 ((int,int) cellii)
        {
            textures = textures_given;

            Texture2D[] hex_textures = new Texture2D[] { textures["gray_hex"], textures["yellow_hex"], textures["blue_hex"], textures["blue_yellow_hex"] };
            Rectangle hex_rec = new Rectangle(new Point(zero_hex_center.X - step_size / 2 + cellii.Item1 * step_size / 2 + cellii.Item2 * step_size,
                    zero_hex_center.Y - (int)Math.Round(step_size * r / 2) + cellii.Item1 * (int)Math.Round(step_size / r))
                    , new Point(step_size, (int)Math.Round(r * step_size)));
            Button new_hex = new Button(hex_rec, hex_textures, Game1.literally_nothing, "hex_in_rec", new string[] { "normal" });
            cells.Add(cellii, new Cell(cellii.Item1, cellii.Item2));
            cells[cellii].self_button = new_hex;
        }
        public void remove_cell_stage1((int, int) cellii)
        {
            cells.Remove(cellii);
        }

        public void set_step_size(int new_step_size)
        {
            step_size = new_step_size;
            set_step_size_cell_self_buttons();
            if (stage == 2)
            {
                add_edge_buttons_stage_2();
            }
        }
        public void set_step_size_cell_self_buttons()
        {
            foreach ((int, int) cellii in cells.Keys)
            {
                cells[cellii].self_button.rectangle = new Rectangle(new Point(zero_hex_center.X - step_size / 2 + cellii.Item1 * step_size / 2 + cellii.Item2 * step_size,
                    zero_hex_center.Y - (int)Math.Round(step_size * r / 2) + cellii.Item1 * (int)Math.Round(step_size / r))
                    , new Point(step_size, (int)Math.Round(r * step_size)));
            }
        }
        /*public void set_step_size_edge_self_buttons()
        {
            foreach ((int, int, int ) edgeiii in edges.Keys)
            {
                if (edges[edgeiii].self_button != null )
                {
                    edges[edgeiii].set_button_vertexes(zero_hex_center, step_size, edge_ratio);
                }
            }
        }*/

        public void create_map_stage1(Dictionary<string, Texture2D> textures_given, HashSet<(int, int)> cells_indexes)
        {
            // base type map, with ports!
            cells = new Dictionary<(int, int), Cell>();
            edges = new Dictionary<(int, int, int), Edge>();
            vertices = new Dictionary<(int, int, bool), Vertex>();
            this.textures_given = textures_given;

            foreach ((int, int) cell in cells_indexes)
                //Add the cells
            {
                cells.Add((cell.Item1, cell.Item2), new Cell(cell.Item1, cell.Item2));
            }
        }

        public void add_edges()
        {
            foreach ((int, int) cell in cells.Keys)
            {

                //Add the edges
                (int, int, int) e1 = (cell.Item1, cell.Item2, 1);
                (int, int, int) e3 = (cell.Item1, cell.Item2 + 1, 9);
                (int, int, int) e5 = (cell.Item1, cell.Item2, 5);
                (int, int, int) e7 = (cell.Item1 + 1, cell.Item2 - 1, 1);
                (int, int, int) e9 = (cell.Item1, cell.Item2, 9);
                (int, int, int) e11 = (cell.Item1 - 1, cell.Item2, 5);

                HashSet<(int, int, int)> maybe_new_edges = new HashSet<(int, int, int)> { e1, e3, e5, e7, e9, e11 };

                foreach ((int, int, int) edge in maybe_new_edges)
                {
                    if (!edges.ContainsKey((edge.Item1, edge.Item2, edge.Item3)))
                    {
                        edges.Add(edge, new Edge(edge.Item1, edge.Item2, edge.Item3));
                    }
                }
            }
        }
        public void add_verteces_and_connect_all()
        { 
            foreach ((int, int) cell in cells.Keys)
            {
                //Add the vertises

                (int, int, bool) v0 = (cell.Item1, cell.Item2, true);
                (int, int, bool) v2 = (cell.Item1 - 1, cell.Item2 + 1, false);
                (int, int, bool) v4 = (cell.Item1 + 1, cell.Item2, true);
                (int, int, bool) v6 = (cell.Item1, cell.Item2, false);
                (int, int, bool) v8 = (cell.Item1 + 1, cell.Item2 - 1, true);
                (int, int, bool) v10 = (cell.Item1 - 1, cell.Item2, false);

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
        }
        public void Add_ports(Dictionary<(int, int, int), string> ports)
        {
            foreach ((int, int, int) edge in ports.Keys)
            {
                edges[(edge.Item1, edge.Item2, edge.Item3)].port = ports[(edge.Item1, edge.Item2, edge.Item3)];
            }
        }
        /*public void Add_cells(HashSet<(int, int)> cells_indexes)
        {
            foreach ((int, int) cell in cells_indexes)
            {
                cells[(cell.Item1, cell.Item2)].base_type = cell.Item3;
            }
        }*/
       
        public void add_cell_resource(HashSet<(int, int, string)> reasources)
        {
            foreach ((int, int, string) resource in reasources)
            {
                //cells[(resource.Item1, resource.Item2)] = 
            }
        }


        public void add_cell_self_buttons_stage1(int step_size_given, Point zero_hex_center_given)
        {
            
            step_size = step_size_given;
            zero_hex_center = zero_hex_center_given;
            textures = textures_given;

            Texture2D[] hex_textures = new Texture2D[] { textures["gray_hex"], textures["yellow_hex"], textures["blue_hex"], textures["blue_yellow_hex"] };
            foreach ((int,int) cellii in cells.Keys)
            {
                Rectangle hex_rec = new Rectangle(new Point(zero_hex_center.X - step_size/2 + cellii.Item1 * step_size/2  + cellii.Item2 * step_size ,
                    zero_hex_center.Y - (int)Math.Round(step_size * r/2) + cellii.Item1* (int)Math.Round(step_size/r) )
                    , new Point(step_size, (int)Math.Round(r*step_size)));
                Button new_hex = new Button(hex_rec, hex_textures, Game1.literally_nothing, "hex_in_rec", new string[] { "normal" });
                cells[cellii].self_button = new_hex;
            }
        }
        public void add_cell_self_buttons_stage2(int step_size_given, Point zero_hex_center_given)
        {
            step_size = step_size_given;
            zero_hex_center = zero_hex_center_given;
            textures = textures_given;

            Texture2D[] hex_textures = new Texture2D[] { textures["gray_hex"], textures["yellow_hex"], textures["blue_hex"], textures["blue_yellow_hex"] };
            foreach ((int, int) cellii in cells.Keys)
            {
                Rectangle hex_rec = new Rectangle(new Point(zero_hex_center.X - step_size / 2 + cellii.Item1 * step_size / 2 + cellii.Item2 * step_size,
                    zero_hex_center.Y - (int)Math.Round(step_size * r / 2) + cellii.Item1 * (int)Math.Round(step_size / r))
                    , new Point(step_size, (int)Math.Round(r * step_size)));
                Texture2D hex_texture;
                if (cells[cellii].is_have_base_type)
                {
                    if (cells[cellii].base_type == "land")
                    {
                        hex_texture = textures["yellow_hex"];
                    }
                    else
                    {
                        hex_texture = textures["blue_hex"];
                    }
                }
                else
                {
                    hex_texture = textures["blue_yellow_hex"];
                }
                Button new_hex = new Button(hex_rec, hex_texture, Game1.literally_nothing, "hex_in_rec", new string[] { "normal" });
                cells[cellii].self_button = new_hex;
            }
        }
        public void add_cell_num_buttons((int, int) cellii, int cube_num)
        {
            cells[cellii].cube_num = cube_num;
            Rectangle hex_rec = cells[cellii].self_button.rectangle;

            //num_buttons.Add()
        }

        public void level_up_to_stage2(int random_land_balance = 0)
        {
        
            this.random_land_balance = random_land_balance;

            foreach ((int, int) cellii in cells.Keys)
            {
                if (cells[cellii].self_button.phase_index == 1)
                {
                    land_cells.Add(cellii);
                    all_cells.Add(cellii);
                }
                if (cells[cellii].self_button.phase_index == 2)
                {
                    sea_cells.Add(cellii);
                    all_cells.Add(cellii);
                }
                if (cells[cellii].self_button.phase_index == 3)
                {
                    sea_land_cells.Add(cellii);
                    all_cells.Add(cellii);
                }
            }

            
            if (check_connectivity(all_cells))
            {
                //stage2 intialization
                stage = 2;
                //create_map_stage1(textures_given, cells_indexes);
                //add_the_edges_and_the_verteces_and_connect_them();
                foreach ((int, int) cellii in cells.Keys)
                {
                    if (sea_cells.Contains(cellii))
                    {
                        cells[cellii].is_have_base_type = true;
                        cells[cellii].base_type = "sea";
                    }
                    else if(land_cells.Contains(cellii))
                    {
                        cells[cellii].is_have_base_type = true;
                        cells[cellii].base_type = "land";
                    }
                    else if(sea_land_cells.Contains(cellii))
                    {
                        cells[cellii].is_have_base_type = false;
                    }
                    else
                    {
                        remove_cell_stage1(cellii);
                    }
                }
                change_button_cells_from_stage1_to_stage2();
                add_edges();
                add_edge_buttons_stage_2();
            }
            else
            {
                Game1.declear("No connection between your cells");
            }
        }
        public void change_button_cells_from_stage1_to_stage2()
        {
            foreach(Cell cell in cells.Values)
            {
                cell.self_button = new Button(cell.self_button.rectangle,
                cell.self_button.all_textures[cell.self_button.phase_index], cell.self_button.action,
                cell.self_button.shape, cell.self_button.activity_phases, cell.self_button.layer);
            }
        }
        
        public void add_edge_buttons_stage_2()
        {
            foreach (Edge edge in edges.Values)
            {
                edge.edge_vertexs = edge.get_button_vertexes(zero_hex_center, step_size, edge_ratio);
                Texture2D[] edge_textures = new Texture2D[] { textures["gray_rec"], textures["green"], textures["red"] };
                if (edge.self_button == null)
                {
                    edge.self_button = new Button(this.game, edge.edge_vertexs, edge_textures, Game1.literally_nothing, new string[] { "normal" }, 2);
                }
                else
                {
                    edge.self_button.set_vertexes(edge.get_button_vertexes(zero_hex_center, step_size, edge_ratio));
                }
                
            }
        }
        
        public string convert_hash_int_int_to_str(HashSet<(int,int)> hii)
        {
            string str = "";
            foreach ((int, int) ii in hii)
            {
                str += "(" + Convert.ToString(ii.Item1) + "," + Convert.ToString(ii.Item2) + ")" + ",";
            }
            return str;
        }
        public bool check_connectivity(HashSet<(int, int)> all_cells)
        {
            // check empty hashset
            (int, int) first_cellii = (0,0);
            foreach ((int, int) cellii in all_cells)
            {
                // you see what I did there?
                first_cellii = cellii;
                break;
            }

            HashSet<(int, int)> first_island = new HashSet<(int, int)>();
            HashSet<(int, int)> first_islands_neighbours = new HashSet<(int, int)>();
            HashSet<(int, int)> next_first_islands_neighbours = new HashSet<(int, int)>();
            first_islands_neighbours.Add(first_cellii);

            HashSet<(int, int)> rule_of_graph = new HashSet<(int, int)>() { (0, 1),(0, -1), (-1,0), (-1, 1), (1, 0), (1, -1) };
            (int, int) new_potential_neighbour;
            while (first_islands_neighbours.Count != 0)
            {
                foreach ((int, int) cellii in first_islands_neighbours)
                {
                   foreach ((int, int) direction in rule_of_graph)
                   {
                        new_potential_neighbour = (cellii.Item1 + direction.Item1, cellii.Item2 + direction.Item2);
                        if (all_cells.Contains(new_potential_neighbour) && 
                            (!first_islands_neighbours.Contains(new_potential_neighbour)) &&
                            (!first_island.Contains(new_potential_neighbour)) &&
                            (!next_first_islands_neighbours.Contains(new_potential_neighbour)))
                        {
                            next_first_islands_neighbours.Add(new_potential_neighbour);
                        }
                   }
                    
                }
                first_island.UnionWith(first_islands_neighbours);
                first_islands_neighbours = next_first_islands_neighbours;
                next_first_islands_neighbours = new HashSet<(int, int)>();
            }
            return (all_cells.Count == first_island.Count);
        }
    }
}
