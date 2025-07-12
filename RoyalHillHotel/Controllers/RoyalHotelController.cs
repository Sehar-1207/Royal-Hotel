using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalHillHotel.Data;
using RoyalHillHotel.Models;
using System.Reflection.Metadata.Ecma335;

namespace RoyalHillHotel.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RoyalHotelController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public RoyalHotelController(ApplicationDbContext db)
        {
            _db = db;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("CheckAdmin")]
        public async Task<IActionResult> CheckAdmin([FromBody] AdminDto admin)
        {
            if (admin == null || string.IsNullOrWhiteSpace(admin.Username) || string.IsNullOrWhiteSpace(admin.Password))
            {
                return BadRequest("Username and Password are required.");
            }

            var existingAdmin = await _db.Admins
                .FirstOrDefaultAsync(a => a.Username == admin.Username && a.Password == admin.Password);

            if (existingAdmin == null)
            {
                return NotFound("Invalid username or password.");
            }

            return Ok("Success");
        }


        //---------------------------------------------Customers---------------------------------------------------------------------------
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("Get-Customers")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _db.Customers.ToListAsync();

            if (customers == null || customers.Count == 0)
            {
                return NotFound();
            }
            return Ok(customers);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("Get-Customer/{id:int}")]
        public async Task<IActionResult> GetCustomersById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Enter Valid id");
            }

            var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("Post-Customer")]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer data is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newCustomer = new Customer
            {
                Name = customer.Name,
                Address = customer.Address,
                Contact = customer.Contact,
                CNIC = customer.CNIC,
                CheckIn = customer.CheckIn,
                CheckOut = customer.CheckOut
            };

            _db.Customers.Add(newCustomer);
            await _db.SaveChangesAsync();

            return Ok(newCustomer);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("UpdateCustomer/{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer cusmodel)
        {
            if (cusmodel == null)
                return BadRequest("Customer data is missing.");

            var customer = await _db.Customers.FindAsync(id);
            if (customer == null)
                return NotFound($"Customer with ID {id} not found.");

            // Update fields
            customer.Name = cusmodel.Name;
            customer.Address = cusmodel.Address;
            customer.Contact = cusmodel.Contact;
            customer.CNIC = cusmodel.CNIC;
            customer.CheckIn = cusmodel.CheckIn;
            customer.CheckOut = cusmodel.CheckOut;

            await _db.SaveChangesAsync();

            return Ok(customer);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("DeleteCustomer/{id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Id........");
            }

            var cid = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (cid == null)
            {
                return NotFound();
            }
            _db.Customers.Remove(cid);
            await _db.SaveChangesAsync();
            return Ok("deleted Successfully");
        }


        //----------------------- Staff ----------------------------------------------------------------------------------------

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("Staff")]
        public async Task<IActionResult> GetStaff()
        {
            var staff = await _db.Staffs.ToListAsync();

            if (staff == null || staff.Count == 0)
            {
                return NotFound("No Information...");
            }
            return Ok(staff);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("Staff/{id:int}")]
        public async Task<IActionResult> GetStaffById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid!!!!! ");
            }

            var staff = await _db.Staffs.FirstOrDefaultAsync(x => x.Id == id);
            if (staff == null)
            {
                return NotFound("No Record");
            }
            return Ok(staff);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("Register-newStaff")]
        public async Task<IActionResult> RegisterStaff([FromBody] Staff staff)
        {
            if (staff == null)
            {
                return BadRequest("Please enter all details properly.");
            }

            var newStaff = new Staff
            {
                Name = staff.Name,
                ContactNumber = staff.ContactNumber,
                DateJoined = staff.DateJoined,
                Role = staff.Role,
                Shift = staff.Shift,
                Salary = staff.Salary
            };

            await _db.Staffs.AddAsync(newStaff);
            await _db.SaveChangesAsync();

            return Ok(newStaff);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpPut("Update/{id:int}")]
        [HttpPut("UpdateStaff/{id}")]
        public async Task<IActionResult> UpdateStaff(int id, [FromBody] Staff updatedStaff)
        {
            if (updatedStaff == null)
            {
                return BadRequest("Staff data is required.");
            }

            if (id != updatedStaff.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            var existingStaff = await _db.Staffs.FirstOrDefaultAsync(s => s.Id == id);
            if (existingStaff == null)
            {
                return NotFound("Staff not found.");
            }

            // Update properties
            existingStaff.Name = updatedStaff.Name;
            existingStaff.ContactNumber = updatedStaff.ContactNumber;
            existingStaff.Shift = updatedStaff.Shift;
            existingStaff.Role = updatedStaff.Role;
            existingStaff.DateJoined = updatedStaff.DateJoined;
            existingStaff.Salary = updatedStaff.Salary;

            // Save changes
            _db.Staffs.Update(existingStaff);
            await _db.SaveChangesAsync();

            return Ok(existingStaff);
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("Delete-Staff/{id:int}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var staff = await _db.Staffs.FirstOrDefaultAsync(x => x.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            _db.Staffs.Remove(staff);
            await _db.SaveChangesAsync();
            return Ok("Successfully deleted");
        }


        //---------------------------------------------------Rooms -------------------------------------------------------------------
        
        [HttpGet("Get-Rooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _db.Rooms.ToListAsync();
            if (rooms == null || !rooms.Any())
                return NotFound("No rooms found.");

            return Ok(rooms);
        }

        
        [HttpGet("Get-Room/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoomById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid room ID.");

            var room = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (room == null)
                return NotFound($"Room with ID {id} not found.");

            return Ok(room);
        }

        
        [HttpPost("Create-Room")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Room>> CreateRoom([FromBody] Room room)
        {
            if (room == null)
                return BadRequest("Room data is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newRoom = new Room
            {
                RoomNo = room.RoomNo,
                RoomType = room.RoomType,
                Price = room.Price,
                Status = room.Status
            };

            await _db.Rooms.AddAsync(newRoom);
            await _db.SaveChangesAsync();

            // return Created result
            return CreatedAtAction(nameof(GetRoomById), new { id = newRoom.Id }, newRoom);
        }


        [HttpPut("Update-Room/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            if (updatedRoom == null || id != updatedRoom.Id)
                return BadRequest("Room ID mismatch or data is invalid.");

            var existingRoom = await _db.Rooms.FindAsync(id);
            if (existingRoom == null)
                return NotFound($"Room with ID {id} not found.");

            // Update fields
            existingRoom.RoomNo = updatedRoom.RoomNo;
            existingRoom.RoomType = updatedRoom.RoomType;
            existingRoom.Price = updatedRoom.Price;
            existingRoom.Status = updatedRoom.Status;

            await _db.SaveChangesAsync();
            return Ok(existingRoom);
        }


        [HttpDelete("Delete-Room/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (room == null)
                return NotFound($"Room with ID {id} not found.");

            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();

            return Ok($"Room with ID {id} deleted.");
        }


        //-----------------------------------------------------Booking-----------------------------------------------------------
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetBookings")]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _db.Bookings.Include(a => a.Customer).Include(b => b.Room).ToListAsync();

            if (bookings == null || bookings.Count == 0)
            {
                return NotFound("No bookings found.");
            }
            return Ok(bookings);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetBookingById/{id:int}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var b = await _db.Bookings.FindAsync(id);
            if (b == null) return NotFound();

            return Ok(new BookingDto
            {
                Id = b.Id,
                CustomerId = b.CustomerId,
                RoomId = b.RoomId,
                Duration = b.Duration,
                IsPaid = b.IsPaid,
                TotalAmount = b.TotalAmount
            });
        }


        [HttpPost("CreateBooking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var customer = await _db.Customers.FindAsync(dto.CustomerId);
            var room = await _db.Rooms.FindAsync(dto.RoomId);
            if (customer == null || room == null) return NotFound("Customer or Room not found.");
            if (room.Status.ToLower() == "occupied") return BadRequest("Room is occupied.");

            var booking = new Booking
            {
                CustomerId = dto.CustomerId,
                RoomId = dto.RoomId,
                Duration = dto.Duration,
                IsPaid = dto.IsPaid,
                TotalAmount = room.Price * dto.Duration
            };

            _db.Bookings.Add(booking);
            room.Status = "Occupied";
            await _db.SaveChangesAsync();

            dto.Id = booking.Id;
            dto.TotalAmount = booking.TotalAmount;
            return Ok(dto);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("UpdateBooking/{id}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingDto bookingDto)
        {
            if (id != bookingDto.Id)
                return BadRequest("Booking ID mismatch.");

            var booking = await _db.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound("Booking not found.");

            var customer = await _db.Customers.FindAsync(bookingDto.CustomerId);
            if (customer == null)
                return NotFound("Customer not found.");

            var newRoom = await _db.Rooms.FindAsync(bookingDto.RoomId);
            if (newRoom == null)
                return NotFound("Room not found.");

            // Room switch check
            if (booking.RoomId != bookingDto.RoomId && newRoom.Status?.ToLower() == "occupied")
                return BadRequest("Selected new room is already occupied.");

            // Switch room
            if (booking.RoomId != bookingDto.RoomId)
            {
                var oldRoom = await _db.Rooms.FindAsync(booking.RoomId);
                if (oldRoom != null)
                    oldRoom.Status = "Available";

                newRoom.Status = "Occupied";
            }

            // Log values
            Console.WriteLine($"🔧 Before Update - Duration: {booking.Duration}, Paid: {booking.IsPaid}, RoomId: {booking.RoomId}");
            Console.WriteLine($"🔧 New values     - Duration: {bookingDto.Duration}, Paid: {bookingDto.IsPaid}, RoomId: {bookingDto.RoomId}");

            // Apply changes
            booking.CustomerId = bookingDto.CustomerId;
            booking.RoomId = bookingDto.RoomId;
            booking.Duration = bookingDto.Duration;
            booking.IsPaid = bookingDto.IsPaid;
            booking.TotalAmount = newRoom.Price * bookingDto.Duration;
            customer.CheckIn = DateTime.Now;

            // Force EF to track update
            _db.Bookings.Update(booking);

            var result = await _db.SaveChangesAsync();
            Console.WriteLine($"💾 SaveChangesAsync result: {result}");

            return Ok("Booking updated successfully.");
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("CancelBooking/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var b = await _db.Bookings.Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == id);
            if (b == null) return NotFound();

            var room = await _db.Rooms.FindAsync(b.RoomId);
            if (room != null) room.Status = "Available";

            b.Customer.CheckOut = DateTime.Now;

            _db.Bookings.Remove(b);
            await _db.SaveChangesAsync();

            return Ok("Cancelled");
        }

        //---------------------------------------- Invoice --------------------------
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var invoices = await _db.Invoices
                .Include(i => i.Booking)
                    .ThenInclude(b => b.Customer)
                .Include(i => i.Booking.Room)
                .ToListAsync();

            if (invoices == null || !invoices.Any())
                return NotFound("No invoices found.");

            return Ok(invoices);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Invoices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoiceById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid invoice ID.");

            var invoice = await _db.Invoices
                .Include(i => i.Booking)
                    .ThenInclude(b => b.Customer)
                .Include(i => i.Booking.Room)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound("Invoice not found.");

            return Ok(invoice);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<IActionResult> PostInvoice([FromBody] InvoiceDto invoiceDto)
        {
            if (invoiceDto == null || invoiceDto.BookingId <= 0)
                return BadRequest("Invalid invoice data.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booking = await _db.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == invoiceDto.BookingId);

            if (booking == null)
                return BadRequest("Booking not found.");

            if (booking.Room == null)
                return BadRequest("Room not found for this booking.");

            var roomCharges = booking.Room.Price * booking.Duration;
            var totalAmount = (double)(roomCharges + invoiceDto.ExtraCharges);

            var invoice = new Invoice
            {
                BookingId = invoiceDto.BookingId,
                InvoiceDate = DateTime.Now,
                RoomCharges = roomCharges,
                ExtraCharges = invoiceDto.ExtraCharges,
                TotalAmount = totalAmount,
                IsPaid = invoiceDto.IsPaid
            };

            await _db.Invoices.AddAsync(invoice);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] InvoiceDto invoiceDto)
        {
            if (invoiceDto == null)
                return BadRequest("Invalid data.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingInvoice = await _db.Invoices.FindAsync(id);
            if (existingInvoice == null)
                return NotFound("Invoice not found.");

            var booking = await _db.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == invoiceDto.BookingId);

            if (booking == null || booking.Room == null)
                return BadRequest("Booking or Room not found.");

            // Calculate updated charges
            var roomCharges = booking.Room.Price * booking.Duration;
            var totalAmount = (double)(roomCharges + invoiceDto.ExtraCharges);

            // Update values
            existingInvoice.BookingId = invoiceDto.BookingId;
            existingInvoice.RoomCharges = roomCharges;
            existingInvoice.ExtraCharges = invoiceDto.ExtraCharges;
            existingInvoice.TotalAmount = totalAmount;
            existingInvoice.IsPaid = invoiceDto.IsPaid;
            // Do NOT update InvoiceDate (keep original)

            _db.Invoices.Update(existingInvoice);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var invoice = await _db.Invoices.FindAsync(id);
            if (invoice == null)
                return NotFound("Invoice not found.");

            _db.Invoices.Remove(invoice);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
