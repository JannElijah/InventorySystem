from dataclasses import dataclass
from datetime import datetime
from typing import Optional

@dataclass
class User:
    user_id: int
    username: str
    password_hash: str
    role: str
    contact_number: Optional[str] = None

@dataclass
class Product:
    product_id: int
    barcode: str
    part_number: str
    brand: str
    description: str
    volume: str
    type: str
    application: str
    purchase_cost: float
    selling_price: float
    stock_quantity: int
    notes: str
    low_stock_threshold: int
    date_created: str
    date_modified: str
    # V2 Fields
    category_id: Optional[int] = None
    target_stock: int = 10
    reorder_point: int = 5
    image_path: Optional[str] = None
    is_active: bool = True

@dataclass
class Transaction:
    transaction_id: int
    product_id: int
    transaction_type: str
    quantity_change: int
    stock_before: int
    stock_after: int
    transaction_date: datetime
    supplier_id: Optional[int]
    # Joined fields
    barcode: Optional[str] = ""
    product_description: Optional[str] = ""
    supplier_name: Optional[str] = ""
    customer_name: Optional[str] = ""
    transaction_value: float = 0.0 # Calculated value
    user_id: Optional[int] = None  # Added in V1

@dataclass
class SaleItem:
    product_id: int
    quantity: int
    unit_price: float
    total: float
    # Helper
    product: Optional[Product] = None

@dataclass
class Sale:
    customer_name: str
    sale_date: datetime
    items: list[SaleItem]
    # Helper
    transaction_id: Optional[int] = None

    @property
    def subtotal(self) -> float:
        return sum(item.total for item in self.items)

@dataclass
class Supplier:
    supplier_id: int
    name: str
    contact_info: str
    email: Optional[str] = ""
    phone: Optional[str] = ""
    is_active: bool = True

@dataclass
class POItem:
    item_id: int
    po_id: int
    product_id: int
    quantity_ordered: int
    quantity_received: int
    unit_cost: float
    # Helper
    product_name: Optional[str] = ""

@dataclass
class PurchaseOrder:
    po_id: int
    supplier_id: int
    order_date: datetime
    status: str # Draft, Ordered, Partially Received, Completed, Cancelled
    total_cost: float
    items: list[POItem] = None
    # Helper
    supplier_name: Optional[str] = ""

@dataclass
class Category:
    category_id: int
    name: str
    description: str

@dataclass
class Customer:
    customer_id: int
    name: str
    phone: str
    email: str
    total_spend: float = 0.0
    is_active: bool = True

@dataclass
class Expense:
    expense_id: int
    category: str
    amount: float
    date: datetime
    description: str

@dataclass
class ChartDataPoint:
    label: str
    value: float

@dataclass
class Notification:
    notification_id: int
    product_id: int
    message: str
    is_read: bool = False
    timestamp: str = ""

