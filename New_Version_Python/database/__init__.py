
from .core import BaseRepository
from .users import UserRepositoryMixin
from .products import ProductRepositoryMixin
from .sales import SalesRepositoryMixin
from .analytics import AnalyticsRepositoryMixin
from .extras import AuditLogMixin, NotificationMixin

class DatabaseRepository(
    BaseRepository,
    UserRepositoryMixin,
    ProductRepositoryMixin,
    SalesRepositoryMixin,
    AnalyticsRepositoryMixin,
    AuditLogMixin,
    NotificationMixin
):
    """
    Facade for the database. 
    Inherits from specialized mixins to provide a unified API.
    """
    def __init__(self, db_path: str = None):
        super().__init__(db_path)
