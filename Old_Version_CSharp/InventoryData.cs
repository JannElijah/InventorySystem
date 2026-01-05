using System.Collections.Generic;

namespace InventorySystem
{
    public static class InventoryData
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>
            {
                // ===== TOYOTA =====
                new Product { ProductID = 101, Barcode = "TOY001", Brand = "TOYOTA", Description = "5W-30 SN/CF (In Can)", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 102, Barcode = "TOY002", Brand = "TOYOTA", Description = "5W-40 SN/CF Fully Synthetic", Volume = "24x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 103, Barcode = "TOY003", Brand = "TOYOTA", Description = "5W-40 SN/CF Fully Synthetic", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 104, Barcode = "TOY004", Brand = "TOYOTA", Description = "ATF T-IV (Type-IV)", Volume = "6x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 105, Barcode = "TOY005", Brand = "TOYOTA", Description = "ATF T-IV (Type-IV) In Can", Volume = "24x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 106, Barcode = "TOY006", Brand = "TOYOTA", Description = "ATF T-IV (Type-IV) In Can", Volume = "6x4L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 107, Barcode = "TOY007", Brand = "TOYOTA", Description = "ATF WS", Volume = "12x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 108, Barcode = "TOY008", Brand = "TOYOTA", Description = "Long Life Coolant (50/50)", Volume = "12x1L", Type = "Coolant", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 109, Barcode = "TOY009", Brand = "TOYOTA", Description = "GL-5 85W90 Differential Gear Oil", Volume = "24x1L", Type = "Gear Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 110, Barcode = "TOY010", Brand = "TOYOTA", Description = "GL-4 75W-90 Manual Transmission Gear Oil", Volume = "24x1L", Type = "Gear Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 111, Barcode = "TOY011", Brand = "TOYOTA", Description = "08880-85146: 15W-40 CI DSL", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 112, Barcode = "TOY012", Brand = "TOYOTA", Description = "08886-85145: 15W-40 CI DSL", Volume = "4x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 113, Barcode = "TOY013", Brand = "TOYOTA", Description = "08880-85156: 20W-50 SN(GAS)", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 114, Barcode = "TOY014", Brand = "TOYOTA", Description = "08880-85155: 20W-50 SN(GAS)", Volume = "4x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 115, Barcode = "TOY015", Brand = "TOYOTA", Description = "08880-85086: 10W-30 SN/CF", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 116, Barcode = "TOY016", Brand = "TOYOTA", Description = "08880-85085: 10W-30 SN/CF", Volume = "4x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 117, Barcode = "TOY017", Brand = "TOYOTA", Description = "CVT Fluid FE (In Can)", Volume = "6x4L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },

                // ===== MITSUBISHI =====
                new Product { ProductID = 201, Barcode = "MIT001", Brand = "MITSUBISHI", Description = "ATF SPIII", Volume = "24x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 202, Barcode = "MIT002", Brand = "MITSUBISHI", Description = "ATF SPIII (In Can)", Volume = "6x4L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 203, Barcode = "MIT003", Brand = "MITSUBISHI", Description = "5W-30 Fully Synthetic ACEA A3/B4-12 Diesel", Volume = "24x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 204, Barcode = "MIT004", Brand = "MITSUBISHI", Description = "5W-40 Fully Synthetic", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 205, Barcode = "MIT005", Brand = "MITSUBISHI", Description = "5W-40 Fully Synthetic", Volume = "24x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 206, Barcode = "MIT006", Brand = "MITSUBISHI", Description = "CVTF J-4 (MZ321008)", Volume = "24x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 207, Barcode = "MIT007", Brand = "MITSUBISHI", Description = "MA-1 Auto Transmission Fluid", Volume = "24x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 208, Barcode = "MIT008", Brand = "MITSUBISHI", Description = "PSF (MZ320743) Power Steering Fluid", Volume = "24x1L", Type = "Power Steering Fluid", StockQuantity = 50, PurchaseCost = 0.00m },

                // ===== BESCO / ISUZU =====
                new Product { ProductID = 301, Barcode = "BES001", Brand = "BESCO", Description = "10W30 API CH-4 JASO DH-1", Volume = "24x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 302, Barcode = "BES002", Brand = "BESCO", Description = "10W30 API CH-4 Duramax Multi Z", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 303, Barcode = "ISU001", Brand = "ISUZU", Description = "ATF Automatic Transmission Fluid", Volume = "24x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 304, Barcode = "ISU002", Brand = "ISUZU", Description = "SAE 15W40", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 305, Barcode = "ISU003", Brand = "ISUZU", Description = "SAE 15W40", Volume = "24x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 306, Barcode = "ISU004", Brand = "ISUZU", Description = "10W30 MULTI Z API CI-4 Synthetic Blend", Volume = "4x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 307, Barcode = "ISU005", Brand = "ISUZU", Description = "10W30 MULTI Z API CI-4 Synthetic Blend", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },

                // ===== NISSAN =====
                new Product { ProductID = 401, Barcode = "NIS001", Brand = "NISSAN", Description = "Matic S ATF", Volume = "24x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 402, Barcode = "NIS002", Brand = "NISSAN", Description = "CVT Fluid NS-3 (In Can)", Volume = "6x4L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 403, Barcode = "NIS003", Brand = "NISSAN", Description = "5W-30 SN Strong Save-X (In Can)", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 404, Barcode = "NIS004", Brand = "NISSAN", Description = "5W-30 SP/GF-6 Fully Synthetic Gasoline", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },

                // ===== HYUNDAI / SUZUKI =====
                new Product { ProductID = 501, Barcode = "HYU001", Brand = "HYUNDAI", Description = "ATF SP-IV", Volume = "12x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 502, Barcode = "HYU002", Brand = "HYUNDAI", Description = "10W-30 API CH-4 Diesel", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 503, Barcode = "HYU003", Brand = "HYUNDAI", Description = "Multi V ATF", Volume = "12x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 504, Barcode = "HYU004", Brand = "HYUNDAI", Description = "C3 5W30 Diesel Ultra", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 505, Barcode = "HYU005", Brand = "HYUNDAI", Description = "C3 5W30 Diesel Ultra", Volume = "3x6L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 506, Barcode = "HYU006", Brand = "HYUNDAI", Description = "G800 5W30 Fully Synthetic Gasoline", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 507, Barcode = "HYU007", Brand = "HYUNDAI", Description = "G800 5W30 Fully Synthetic Gasoline", Volume = "4x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 508, Barcode = "HYU008", Brand = "HYUNDAI", Description = "D500 10W30 CI-4 Diesel", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 509, Barcode = "HYU009", Brand = "HYUNDAI", Description = "D500 10W30 CI-4 Diesel", Volume = "3x6L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 510, Barcode = "SUZ001", Brand = "SUZUKI", Description = "SW 30 SG API", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },

                // ===== CORTECO =====
                new Product { ProductID = 601, Barcode = "COR001", Brand = "CORTECO", Description = "SAE 15W-40 GAS/DSL", Volume = "24x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 602, Barcode = "COR002", Brand = "CORTECO", Description = "SAE 15W-40 GAS/DSL", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 603, Barcode = "COR003", Brand = "CORTECO", Description = "SAE 15W-40 GAS/DSL", Volume = "1Pailx18L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 604, Barcode = "COR004", Brand = "CORTECO", Description = "AWS 68 Hydraulic Fluid", Volume = "1Pailx18L", Type = "Hydraulic Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 605, Barcode = "COR005", Brand = "CORTECO", Description = "Dexron III G", Volume = "24x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },

                // ===== HONDA =====
                new Product { ProductID = 701, Barcode = "HON001", Brand = "HONDA", Description = "Protech Ultra 4AT 10W30 Fully Synthetic", Volume = "12x0.8L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 702, Barcode = "HON002", Brand = "HONDA", Description = "ATF DW-1", Volume = "24x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 703, Barcode = "HON003", Brand = "HONDA", Description = "CVTF/Continuously Variable Transmission", Volume = "24x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 704, Barcode = "HON004", Brand = "HONDA", Description = "PSF/Power Steering Fluid", Volume = "24x1L", Type = "Power Steering Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 705, Barcode = "HON005", Brand = "HONDA", Description = "MTF Manual Transmission Fluid", Volume = "24x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 706, Barcode = "HON006", Brand = "HONDA", Description = "10W-30 SN I-VTEC", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 707, Barcode = "HON007", Brand = "HONDA", Description = "10W-30 SP Mineral Bronze", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 708, Barcode = "HON008", Brand = "HONDA", Description = "10W-30 SL VTEC LEV", Volume = "24x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 709, Barcode = "HON009", Brand = "HONDA", Description = "0W-20 SP Fully Synthetic", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 710, Barcode = "HON010", Brand = "HONDA", Description = "0W-20 SP Fully Synthetic", Volume = "24x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 711, Barcode = "HON011", Brand = "HONDA", Description = "0W-20 SN Fully Synthetic", Volume = "24x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 712, Barcode = "HON012", Brand = "HONDA", Description = "5W-30 SP Silver Semi-Synthetic", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 713, Barcode = "HON013", Brand = "HONDA", Description = "5W-30 SP Silver Semi-Synthetic", Volume = "24x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 714, Barcode = "HON014", Brand = "HONDA", Description = "5W-30 SN Semi-Synthetic", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 715, Barcode = "HON015", Brand = "HONDA", Description = "5W-30 SN Semi-Synthetic", Volume = "24x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 716, Barcode = "HON016", Brand = "HONDA", Description = "5W-40 SL Fully Synthetic", Volume = "6x4L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 717, Barcode = "HON017", Brand = "HONDA", Description = "HCF-2 CVT Fluid-2", Volume = "6x3.5L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 718, Barcode = "HON018", Brand = "HONDA", Description = "HCF-2 CVT Fluid-2", Volume = "24x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 719, Barcode = "HON019", Brand = "HONDA", Description = "Long Life Coolant Pre-Mix Type-1 (Green)", Volume = "24x1L", Type = "Coolant", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 720, Barcode = "HON020", Brand = "HONDA", Description = "All Season Anti-Freeze Coolant Type-2", Volume = "24x1L", Type = "Coolant", StockQuantity = 50, PurchaseCost = 0.00m },

                // ===== YAMALUBE =====
                new Product { ProductID = 801, Barcode = "YAM001", Brand = "YAMALUBE", Description = "Blue Core 10w40 Semi Synthetic For Scooter", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 802, Barcode = "YAM002", Brand = "YAMALUBE", Description = "Red 4AT SAE 10w40 For Scooter", Volume = "12x0.8L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                
                // ===== VESLEE =====
                new Product { ProductID = 901, Barcode = "VES001", Brand = "VESLEE", Description = "Rad Coolant Red", Volume = "12x1L", Type = "Coolant", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 902, Barcode = "VES002", Brand = "VESLEE", Description = "Rad Coolant Green", Volume = "12x1L", Type = "Coolant", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 903, Barcode = "VES003", Brand = "VESLEE", Description = "Rad Coolant Red", Volume = "8x2L", Type = "Coolant", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 904, Barcode = "VES004", Brand = "VESLEE", Description = "Rad Coolant Green", Volume = "8x2L", Type = "Coolant", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 905, Barcode = "VES005", Brand = "VESLEE", Description = "Rad Coolant Red", Volume = "6x4L", Type = "Coolant", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 906, Barcode = "VES006", Brand = "VESLEE", Description = "Rad Coolant Green", Volume = "6x4L", Type = "Coolant", StockQuantity = 50, PurchaseCost = 0.00m },

                // ===== FORD =====
                new Product { ProductID = 1001, Barcode = "FOR001", Brand = "FORD", Description = "5W-30 Fully Synthetic Gasoline", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 1002, Barcode = "FOR002", Brand = "FORD", Description = "SAE 5W-30 F-150 Diesel", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 1003, Barcode = "FOR003", Brand = "FORD", Description = "10W-30 Super Duty Diesel", Volume = "12x1L", Type = "Engine Oil", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 1004, Barcode = "FOR004", Brand = "FORD", Description = "Mercon V Automatic Transmission", Volume = "12x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
                new Product { ProductID = 1005, Barcode = "FOR005", Brand = "FORD", Description = "Mercon LV Automatic Transmission", Volume = "12x1L", Type = "Transmission Fluid", StockQuantity = 50, PurchaseCost = 0.00m },
            };
        }
    }
}